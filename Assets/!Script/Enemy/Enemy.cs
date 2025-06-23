using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Enemy : Character
{
    private Player player;
    GameObject wayPoint1;
    bool onWayPoint1;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;
    bool atkDelay;

    float viewAngle = 360;
    int rayCount = 20;
    float rayDistance = 4f;

    public bool dead;

    private Transform lastTarget = null;
    private bool isChasingPlayer = false;

    Vector3 randomTarget;
    float wanderCooldown = 0f;

    Vector3 lastPosition;
    float stuckTimer = 0f;
    float stuckThreshold = 0.1f; // 動いてないとみなす距離
    float stuckTimeLimit = 2f;   // この秒数動けなかったら「スタック」とみなす

    public EnemySpawn enemySpawn;

    public bool isDamaged = false;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        MaxHp = UnityEngine.Random.Range(10,30) * LVL;
        
        damage = 10;
        base.Start();
        wayPoint1 = GameObject.Find("WayPoint1");
    }

    protected override void FixedUpdate()
    {
        if (dead) return;
        speed = 3;
        healthImage.transform.LookAt(Camera.main.transform.position);
        rb.velocity = transform.forward * speed;

        if(rb.velocity.magnitude >= 0.01f)
        {
            animator.SetBool("attack", false);
            animator.SetBool("run", true);
        }
        if (HP <= 0)
        {
            animator.SetBool("dead", true);
            dead = true;
            rb.isKinematic = true;
            player.LVLGauge(1);
            CoinManager.AddCoin(1 + LVL);
            enemySpawn.RemoveEnemy(this);
        }

        if (!isChasingPlayer)
        {
            wanderCooldown -= Time.deltaTime;
            if (wanderCooldown <= 0f)
            {
                PickRandomDirection();
                wanderCooldown = UnityEngine.Random.Range(2f, 5f); // 次に動き出すまでの間隔
            }
        }
        ViewAction();
    }


    void PickRandomDirection()
    {
        float radius = 7f;
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
        randomTarget = new Vector3(transform.position.x + randomOffset.x, transform.position.y, transform.position.z + randomOffset.y);
    }

    void ViewAction()
    {
        if (dead) return;
        Vector3 viewPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        float halfAngle = viewAngle / 2f;

        bool hitTarget = false; // ← ループ外で初期化！
       
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 dir = rotation * transform.forward;

            if (Physics.Raycast(viewPos, dir, out RaycastHit hit, rayDistance))
            {
                
                if (hit.collider.CompareTag("Player") && hit.collider.name == "body")
                {
                    
                    hitTarget = true;

                    lastTarget = hit.collider.transform;
                    isChasingPlayer = true;

                    Vector3 targetDir = lastTarget.position - transform.position;
                    targetDir.y = 0f;

                    if (targetDir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                    }
                    float distance = hit.distance;
                    if (distance <= 1.5f)
                    {
                        rb.velocity = Vector3.zero;
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        isAttacking = true;
                        playAnim = true;
                    }

                    break;
                }

            }

            Debug.DrawRay(viewPos, dir * rayDistance, Color.red);
        }
        if (!hitTarget)
        {
            if (isChasingPlayer && lastTarget != null)
            {
                
                // プレイヤーを追い続ける
                Vector3 chaseDir = lastTarget.position - transform.position;
                chaseDir.y = 0f;

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(chaseDir),
                    Time.deltaTime * 5f
                );

                if (isAttacking)
                {
                    rb.velocity = Vector3.zero;
                    return;
                }
                rb.velocity = transform.forward * speed;

                animator.SetBool("run", true);
                animator.SetBool("attack", false);


                // プレイヤーとの距離が一定以上離れたら追跡やめる
                float dist = Vector3.Distance(transform.position, lastTarget.position);
                if (dist > 5f)
                {
                    isChasingPlayer = false;
                    lastTarget = null;
                }

                return; // WayPoint 処理に入らないよう return
            }

            if (rb.velocity != Vector3.zero) { rb.velocity = transform.forward * speed; }

            // ===== 新しい徘徊処理 =====
            Vector3 dir = randomTarget - transform.position;
            dir.y = 0f;

            if (dir.magnitude > 0.5f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
                rb.velocity = transform.forward * speed;

                animator.SetBool("run", true);
                animator.SetBool("attack", false);
            }
            else
            {
                rb.velocity = Vector3.zero;
                animator.SetBool("run", false);
            }
        }

        float movedDistance = Vector3.Distance(transform.position, lastPosition);

        if (movedDistance < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckTimeLimit)
            {
                // === スタックした！止めて次の行動へ ===
                rb.velocity = Vector3.zero;
                animator.SetBool("run", false);

                stuckTimer = 0f;
                PickRandomDirection(); // 方向転換またはランダム移動先再設定
                wanderCooldown = UnityEngine.Random.Range(1f, 3f); // 少し待ってから動く
            }
        }
        else
        {
            stuckTimer = 0f; // 動いてたらタイマーリセット
        }

        lastPosition = transform.position;

    }

    private void UpdateFillAmount(Image image, ref float currentRate, float targetRate, float duration)
    {
        // 0〜1の範囲に制限
        targetRate = Mathf.Clamp01(targetRate);

        // DOTweenでFillAmountのアニメーション
        image.DOFillAmount(targetRate, duration);

        // currentRateの更新
        currentRate = targetRate;
    }

    public void SetDamage(float _damage)
    {
        if(dead) return;
        HP -= _damage;
        float targetRate = HcurrentRate - _damage / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
        GameObject damageText = Instantiate(damageNotation, transform.Find("UI/healthImage"));
        damageText.GetComponent<Text>().color = new Color32(255,255,255,255) ;
        damageText.GetComponent<Text>().text = _damage.ToString("f1");
        Destroy(damageText, 1);

    }

    CancellationTokenSource cts = new CancellationTokenSource();

    private void OnDestroy()
    {
        cts.Cancel();
    }

    public async void SetAttributeDamage(float _damage, int attribute, Color32 color)
    {
        if (dead) return;
        for (int i = 0; i < attribute; i++)
        {
            if (dead) return;
            HP -= _damage;
            float targetRate = HcurrentRate - _damage / MaxHp;
            UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
            GameObject damageText = Instantiate(damageNotation, transform.Find("UI/healthImage"));
            damageText.GetComponent<Text>().color = color;
            damageText.GetComponent<Text>().text = _damage.ToString("f3");
            Destroy(damageText, 1);

            try
            {
                await UniTask.Delay(2000, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
                // 破棄時にキャンセルされた場合はループを抜ける
                return;
            }
        }
    }

   
    public void Dead()
    {
        Destroy(gameObject);
        GetComponent<Collider>().enabled = false;
    }
}
