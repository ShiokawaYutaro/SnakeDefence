using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Boss : Enemy
{
    [SerializeField] private GameObject warningLine;

    //private bool isChargingAttack = false;
    private readonly float attackArea = 5;

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
        /// 間合いを取る
        /// </summary>
        TakeDistance,
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

    enum AttackType
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

        Max
    }

    //AttackType attackTypeCategory;

    protected override void Start()
    {
        warningLine.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //MaxHp = UnityEngine.Random.Range(10,);
        MaxHp = 200;
        damage = 50;
        speed = 3;
        SetUp();
    }

    protected override void FixedUpdate()
    {
        //死んでたらリターン
        if (dead) return;


        healthImage.transform.LookAt(Camera.main.transform.position);

        ActionProcess();

        if (HP <= 0)
        {
            animator.SetBool("dead", true);
            dead = true;
            rb.isKinematic = true;
            player.LVLGauge(1);
            CoinManager.AddCoin(1 + LVL);
        }


    }

    private async void ActionProcess()
    {
        //首を動かす
        RotateTowardsPlayer();
        //アクションを実行するかどうか [実行しないならリターン]
        if (!ExecuteAction()) return;
        //ランダムでアクションを発動させる（もう少し確率をいじったほうがいい）
        actionCategory = (Action)UnityEngine.Random.Range(0, (int)Action.Max);
        switch (actionCategory)
        {
            case Action.Idel:
                Debug.Log("待機");
                await StartIdel();
                break;
            case Action.Observe:
                Debug.Log("観察");
                await StartObserve();
                break;
            case Action.TakeDistance:
                Debug.Log("間合いを取る");
                await StartTakeDistance();
                break;
            case Action.Chase:
                Debug.Log("追う");
                break;
            case Action.Attack:
                Debug.Log("攻撃発動");
                await StartAttack(UnityEngine.Random.Range(0, (int)AttackType.Max));
                break;
        }

        //アクションが終わったら
        action = false;

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
    /// <summary>
    /// 待機状態になって休憩するようにしたいなぁ（モウハンみたいに）
    /// </summary>
    /// <returns></returns>
    private async UniTask StartIdel()
    {

    }
    /// <summary>
    /// 敵を観察する（なんかかっこいいから！）
    /// </summary>
    /// <returns></returns>
    private async UniTask StartObserve()
    {
        animator.SetBool("観察",true);
    }
    /// <summary>
    /// 後ろに回避して間合いを取る
    /// </summary>
    /// <returns></returns>
    private async UniTask StartTakeDistance()
    {

        //プレイヤーから一定の距離を取る処理（ワンちゃん崖に落ちのでどうしよう）
        //崖に落ちるくらいなら地面の中心に戻す

        //プレイヤーとの距離が遠ければ外れる
        if (Vector3.Distance(transform.position, player.transform.position) >= attackArea) return;

        //後ろの距離を取る地点の取得
        Vector3 fallPoint = transform.localPosition + -transform.forward * 4f;

        if (!CheckGrounded(fallPoint/2))
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

    #region 攻撃関係

    /// <summary>
    /// 攻撃をする範囲と攻撃の実行
    /// </summary>
    /// <returns></returns>
    public async UniTask StartAttack(int attackType)
    {
        //プレイヤーが近くにいなければ攻撃しない
        if (Vector3.Distance(transform.position, player.transform.position) >= attackArea) return;

        switch ((AttackType)attackType)
        {
            case AttackType.Going:
                await GoingAttack();
                break;
            case AttackType.LongRange:
                await LongRangeAttack();
                break;
            case AttackType.Counter:
                await CounterAttack();
                break;
        }
        
    }

    private async UniTask GoingAttack()
    {

    }

    private async UniTask LongRangeAttack()
    {

    }
    /// <summary>
    /// プレイヤーが近づいて攻撃してくるのを攻撃する
    /// </summary>
    /// <returns></returns>
    private async UniTask CounterAttack()
    {
        //ここの文の書き方がきもいからなんか変えたい
        const float attackTime = 3;
        const string attackName = "攻撃カウンター";

        //攻撃のチャージが完了するかどうか
        if (await ChargeTime(attackTime, attackName)) return;
        // 攻撃の実行
        Attack(attackName); 

        // アニメーションの終了を待つ（基底のクラスの関数）
        await WaitUntilAnimationStateExits(attackName); // ←"Attack"はアニメーターのステート名
        //終了したら攻撃範囲の表示を消す
        warningLine.SetActive(false);
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
        warningLine.SetActive(true);

        Image frontImage = warningLine.transform.Find(warningLineName).GetComponent<Image>();

        //攻撃のチャージ時間
        while (currentChargeTime <= time)
        {
            currentChargeTime += Time.deltaTime;
            frontImage.fillAmount = currentChargeTime / time;
            await UniTask.DelayFrame(1);
        }

        return true;
    }

    #endregion

    /// <summary>
    /// 首をプレイヤーに向かせる
    /// </summary>
    private void RotateTowardsPlayer()
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
}
