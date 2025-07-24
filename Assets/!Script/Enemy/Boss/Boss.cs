using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;

public enum AttackType
{
    /// <summary>
    /// 攻撃を当てに行く
    /// </summary>
    Going,
    /// <summary>
    /// 遠距離攻撃
    /// </summary>
    LongRange,
    /// <summary>
    /// 差し返し
    /// </summary>
    Counter,
    /// <summary>
    /// 間合いを取る
    /// </summary>
    TakeDistance,

    Max
}

public class Boss : Enemy
{
    [SerializeField] private GameObject[] warningLine;

    //private bool isChargingAttack = false;
    private readonly float attackArea = 3;

    [SerializeField] Transform neck;

    float actionTime;
    bool action;

    enum Action
    {
        /// <summary>
        /// 待機
        /// </summary>
        Idel,
        /// <summary>
        /// 様子を見る
        /// </summary>
        Observe,
        /// <summary>
        /// 追う
        /// </summary>
        Chase,
        /// <summary>
        /// 攻撃する
        /// </summary>
        Attack,

        Max
    }

    Action actionCategory;

    

   // public AttackType attackType;

    private BossState currentState;
    private BossState nextState;

    private Dictionary<Action, BossState> stateMap;
    private Dictionary<AttackType, AttackStrategy> attackStrategies;
    //AttackType attackTypeCategory;

    protected override void Start()
    {
        // ステート登録
        stateMap = new Dictionary<Action, BossState>
        {
            { Action.Idel, new IdelState() },
            { Action.Observe, new ObserveState() },
            { Action.Chase, new ChaseState() },
            { Action.Attack, new AttackState() },
        };

        currentState = stateMap[Action.Idel];

        attackStrategies = new Dictionary<AttackType, AttackStrategy>
        {
            { AttackType.Going, new GoingAttack() },
            { AttackType.LongRange, new LongRangeAttack() },
            { AttackType.Counter, new CounterAttack() },
            { AttackType.TakeDistance, new TakeDistanceState() }
        };

        for (int i = 0; i < warningLine.Length; i++)
        {
            warningLine[i].SetActive(false);
        }
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //MaxHp = UnityEngine.Random.Range(10,);
        MaxHp = 200;
        damage = 50;
        defence = 30;
        speed = 3;
        SetUp();
    }

    protected override void FixedUpdate()
    {
        //死んでたらリターン
        if (dead) return;
        StateTick().Forget();

        healthImage.transform.LookAt(Camera.main.transform.position);

        //ActionProcess();

        if (HP <= 0)
        {
            animator.SetBool("dead", true);
            dead = true;
            rb.isKinematic = true;
            player.LVLGauge(1);
            CoinManager.instance.AddCoin(1 + LVL);
        }


    }

    /// <summary>
    /// アクションを実行する時間
    /// </summary>
    /// <returns></returns>
    private bool ExecuteAction()
    {

        if (action) return false;

        actionTime += Time.deltaTime;
        float actionInterval = UnityEngine.Random.Range(5, 10);
        if (actionTime >= actionInterval)
        {
            actionTime = 0;
            action = true;
            return true;
        }
        return false;
    }

    private Action previousAction = Action.Max;

    private async UniTask StateTick()
    {
        RotateTowardsPlayer();

        if (!ExecuteAction()) return;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        Dictionary<Action, float> actionWeights = new();

        if (playerDistance < 6f)
        {
            // プレイヤーが近い：攻撃っぽい行動を強調
            actionWeights[Action.Attack] = 50f;
            actionWeights[Action.Chase] = 30f;
            actionWeights[Action.Observe] = 15f;
        }
        else
        {
            // プレイヤーが遠い：様子見や接近系が中心
            actionWeights[Action.Idel] = 50f;
            actionWeights[Action.Chase] = 40f;
            actionWeights[Action.Observe] = 25f;
        }

        // 同じ行動を避けながらランダム選出（重み付き）
        Action nextAction = GetRandomWeighted(actionWeights, previousAction);

        previousAction = nextAction;
        nextState = stateMap[nextAction];
        await SetNextState(nextState);
    }

    private T GetRandomWeighted<T>(Dictionary<T, float> weights, T exclude)
    {
        // 1. 除外する要素を省いた辞書を作成
        var filteredWeights = weights
            .Where(kvp => !EqualityComparer<T>.Default.Equals(kvp.Key, exclude))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        // 2. 合計重み
        float totalWeight = filteredWeights.Values.Sum();

        // 3. ランダム値を生成
        float rand = UnityEngine.Random.Range(0f, totalWeight);

        // 4. 累積して選ぶ
        float cumulative = 0f;
        foreach (var kvp in filteredWeights)
        {
            cumulative += kvp.Value;
            if (rand <= cumulative)
                return kvp.Key;
        }

        // 念のため
        return filteredWeights.Keys.First();
    }


    public async UniTask SetNextState(BossState nextState)
    {
        if (currentState != null)
        {
            await currentState.Exit(this);
        }

        currentState = nextState;

        if (currentState != null)
        {
            await currentState.Enter(this);
            await currentState.Execute(this);
        }

        action = false;
    }
    /// <summary>
    /// 追いかける
    /// </summary>
    /// <returns></returns>
    public async UniTask StartChase()
    {
        while (true)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            // プレイヤーとの距離チェック
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log($"プレイヤーとの距離: {distance}");

            if (distance < attackArea)
            {
                // 攻撃に移るなど
                await SetNextState(new AttackState());
                break;
            }
            else
            {
                rb.velocity = dir * speed * 2;
            }

            // 0.1秒ごとにチェック（負荷軽減）
            await UniTask.Delay(100);
        }
    }

    #region 攻撃関係

    /// <summary>
    /// 攻撃をする範囲と攻撃の実行
    /// </summary>
    /// <returns></returns>
    private AttackType lastAttackType;

    public async UniTask StartAttack()
    {
        rb.velocity = Vector3.zero;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // 距離に応じた攻撃の重み
        Dictionary<AttackType, float> attackWeights = new();

        if (playerDistance < attackArea)
        {
            attackWeights[AttackType.Going] = 50f;
            attackWeights[AttackType.Counter] = 30f;
            attackWeights[AttackType.TakeDistance] = 10f;
        }
        else
        {
            attackWeights[AttackType.LongRange] = 1;
        }

        // 同じ攻撃を避けてランダム選出
        var selectedType = GetRandomWeighted(attackWeights, lastAttackType);

        // 念のためもう一回回避（任意）
        if (selectedType == lastAttackType)
        {
            Debug.Log("同じ攻撃だったので当てに行く攻撃に切り替え");
            selectedType = AttackType.Going;
        }

        if (attackStrategies.TryGetValue(selectedType, out var strategy))
        {
            lastAttackType = selectedType;
            await strategy.Execute(this);
        }
    }

    public async UniTask GoingAttack()
    {
        //ここの文の書き方がきもいからなんか変えたい
        const float attackTime = 1;
        const string attackName = "攻撃当てる";

        //攻撃のチャージが完了するかどうか
        if (!await ChargeTime(attackTime, attackName)) return;
        // プレイヤーがぎりかわせる攻撃の実行
        Attack(attackName);

        // アニメーションの終了を待つ（基底のクラスの関数）
        await WaitUntilAnimationStateExits(attackName); // ←"Attack"はアニメーターのステート名
        //終了したら攻撃範囲の表示を消す
        for (int i = 0; i < warningLine.Length; i++)
        {
            warningLine[i].SetActive(false);
        }
           
    }

    public async UniTask LongRangeAttack()
    {
        //ここの文の書き方がきもいからなんか変えたい
        const float attackTime = 2;
        const string attackName = "攻撃遠距離";

        //成功したら、攻撃のチャージが完了するかどうか
        //if (!await ChargeTime(attackTime, attackName)) return;
        // プレイヤーがぎりかわせる攻撃の実行
        Attack(attackName);

        // アニメーションの終了を待つ（基底のクラスの関数）
        await WaitUntilAnimationStateExits(attackName); // ←"Attack"はアニメーターのステート名
        //終了したら攻撃範囲の表示を消す
        for (int i = 0; i < warningLine.Length; i++)
        {
            warningLine[i].SetActive(false);
        }
    }

    private bool isCounter = false;
    private bool isCounterWait = false;
    /// <summary>
    /// プレイヤーが近づいて攻撃してくるのを攻撃する
    /// </summary>
    /// <returns></returns>
    public async UniTask CounterAttack()
    {
        //ここの文の書き方がきもいからなんか変えたい
        const float attackTime = 0.8f;
        const string attackName = "攻撃カウンター";

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("攻撃カウンター待ち"))
        {
            Debug.Log("すでにカウンター待機中なので入らない");
            return;
        }


        //カウンター状態に入る
        if (!await CheckCounter()) return;
        
        //成功したら、攻撃のチャージが完了するかどうか
        if (!await ChargeTime(attackTime, attackName)) return;
        // プレイヤーがぎりかわせる攻撃の実行
        Attack(attackName); 

        // アニメーションの終了を待つ（基底のクラスの関数）
        await WaitUntilAnimationStateExits(attackName); // ←"Attack"はアニメーターのステート名
        //終了したら攻撃範囲の表示を消す
        for (int i = 0; i < warningLine.Length; i++)
        {
            warningLine[i].SetActive(false);
        }
        animator.SetBool("攻撃カウンター待ち", false);
    }
    public async UniTask<bool> CheckCounter()
    {
        isCounter = false;
        isCounterWait = true;

        animator.SetBool("攻撃カウンター待ち", true);

        // 攻撃されるまで最大3秒待つ
        float time = 0f;
        const float maxTime = 3f;

        while (!isCounter && time < maxTime)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
        }

        if (!isCounter)
        {
            // 成功しなかった
            Debug.Log("カウンター失敗");
            isCounterWait = false;
            animator.SetBool("攻撃カウンター待ち", false);
            return false;
        }

        // 成功済みなので、アニメーション側で処理継続
        return true;
    }

    /// <summary>
    /// 攻撃時間とチャージ画像
    /// </summary>
    /// <param name="time"></param>
    /// <param name="warningLineName"></param>
    /// <returns></returns>
    private async UniTask<bool> ChargeTime(float time,string warningLineName)
    {
        float currentChargeTime = 0f;
        GameObject warningType = null;
        for (int i = 0; i < warningLine.Length; i++)
        {
            if (warningLineName == warningLine[i].gameObject.name)
            {
                warningType = warningLine[i];
            }            
        }
        warningType.SetActive(true);
        Image frontImage =  warningType.transform.Find("frontImage").GetComponent<Image>();

        //攻撃のチャージ時間
        while (currentChargeTime <= time)
        {
            currentChargeTime += Time.deltaTime;
            frontImage.fillAmount = currentChargeTime / time;
            await UniTask.DelayFrame(1);
        }

        return true;
    }
    public async UniTask StartTakeDistance()
    {

        //プレイヤーから一定の距離を取る処理（ワンちゃん崖に落ちのでどうしよう）
        //崖に落ちるくらいなら地面の中心に戻す×

        //後ろの距離を取る地点の取得
        Vector3 fallPoint = transform.localPosition + -transform.forward * 4f;

        if (!CheckGrounded(fallPoint / 2))
        {
            Debug.Log("後ろには飛べない");
            return;
        }

        animator.SetTrigger("takeDistance");

        // 400ms待つ
        await UniTask.Delay(400);

        //個々の瞬間だけ一瞬重くなる
        transform.DOLocalMove(fallPoint, 1f);

    }
    #endregion
    /// <summary>
    /// 首をプレイヤーに向かせる
    /// </summary>
    public void RotateTowardsPlayer()
    {
        if (actionCategory == Action.Attack) return;
        Vector3 targetDir = player.transform.position - transform.position;
        targetDir.y = 0f; // 水平方向のみに限定

        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        
        //首の回る最大数
        float maxAngle = 40;
        if (Mathf.Abs(angle) <= maxAngle)
        {
            // 首だけで向く（角度制限内）
            neck.transform.rotation = Quaternion.LookRotation(player.transform.position - neck.position);
        }
        else
        {
            // 首の範囲を超えたら体をゆっくり回す
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3f); // ←回転速度
        }
    }

    CancellationTokenSource cts = new CancellationTokenSource();

    private void OnDestroy()
    {
        cts.Cancel();
    }

    public override void Dead()
    {
        Destroy(gameObject);
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "武器" && other.transform.root.tag == "Player")
        {
            if(isCounterWait)
            {
                isCounter = true;
            }
        }
    }
}
