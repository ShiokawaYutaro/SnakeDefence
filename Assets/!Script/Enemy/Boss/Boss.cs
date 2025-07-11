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
    [SerializeField] private float attackChargeTime = 3f;

    private float currentChargeTime = 0f;
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

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //MaxHp = UnityEngine.Random.Range(10,);
        MaxHp = 200;
        damage = 50;
        SetUp();
    }

    protected override void FixedUpdate()
    {
        //死んでたらリターン
        if (dead) return;

        speed = 3;
        healthImage.transform.LookAt(Camera.main.transform.position);
        //rb.velocity = transform.forward * speed;

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
        //アクションを実行するかどうか
        //実行しないならリターン
        if (!ExecuteAction()) return;
        //ランダムでアクションを発動させる（もう少し確率をいじったほうがいい）
        actionCategory = (Action)UnityEngine.Random.Range(0, (int)Action.Max);
        switch (actionCategory)
        {
            case Action.Idel:
                Debug.Log("待機");

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
                await StartAttackWarning();
                break;
        }

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

    private async UniTask StartTakeDistance()
    {

        //プレイヤーから一定の距離を取る処理（ワンちゃん崖に落ちのでどうしよう）
        //崖に落ちるくらいなら地面の中心に戻す

        //プレイヤーとの距離が遠ければ外れる
        //if (Vector3.Distance(transform.position, player.transform.position) >= attackArea) return;

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

    /// <summary>
    /// 攻撃をする範囲と攻撃の実行
    /// </summary>
    /// <returns></returns>
    public async UniTask StartAttackWarning()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= attackArea) return;
        currentChargeTime = 0f;
        warningLine.SetActive(true);

        Image frontImage = warningLine.transform.Find("frontImage").GetComponent<Image>();

        while (currentChargeTime <= attackChargeTime)
        {
            currentChargeTime += Time.deltaTime;
            frontImage.fillAmount = currentChargeTime / attackChargeTime;
            await UniTask.DelayFrame(1);
        }

        Debug.Log("攻撃します！");
        Attack(); // アニメーション実行（Trigger）

        // アニメーションの終了を待つ
        await WaitUntilAnimationStateExits("攻撃１"); // ←"Attack"はアニメーターのステート名

        warningLine.SetActive(false);
    }
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
    /// <summary>
    /// 体を全体絶対にプレイヤーに向かせる
    /// </summary>
    private void LookPlayer()
    {
        Vector3 targetDir = player.transform.position;
        targetDir.y = 0f; // 水平方向のみに限定

        transform.DORotate(targetDir, 1);

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
