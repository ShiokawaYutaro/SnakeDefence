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

    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;

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
    float stuckThreshold = 0.01f; // 動いてないとみなす距離
    float stuckTimeLimit = 2f;   // この秒数動けなかったら「スタック」とみなす

    public EnemySpawn enemySpawn;

    public bool isDamaged = false;
    bool isGrounded = false;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        MaxHp = UnityEngine.Random.Range(10,30) * LVL;
        
        damage = 10;
        base.Start();
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
        }
        ViewAction();
        if (!isChasingPlayer)
        {
            wanderCooldown -= Time.deltaTime;
            if (wanderCooldown <= 0f)
            {
                PickRandomDirection();
                wanderCooldown = UnityEngine.Random.Range(2f, 5f); // 次に動き出すまでの間隔
            }
        }
       
    }


    void PickRandomDirection()
    {
        float radius = 7f;
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
        randomTarget = new Vector3(transform.position.x + randomOffset.x, transform.position.y, transform.position.z + randomOffset.y);
    }

    void Attack()
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("run", false);
        animator.SetTrigger("attack");
        isAttacking = true;
        playAnim = true;
    }
    /// <summary>
    /// 徘徊する
    /// </summary>
    void Wandering()
    {
        // ===== 新しい徘徊処理 =====
        Vector3 dir = randomTarget - transform.position;
        dir.y = 0f;

        if (dir.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            rb.velocity = transform.forward * speed;

            animator.SetBool("run", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("run", false);
        }
    }
    /// <summary>
    /// 移動中スタックしたら
    /// </summary>
    void Stuck()
    {
        float movedDistance = Vector3.Distance(transform.position, lastPosition);

        if (movedDistance < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckTimeLimit)
            {
                // === スタックした！止めて次の行動へ ===
                rb.velocity = Vector3.zero;
                animator.SetBool("run", false);
                isChasingPlayer = false;
                stuckTimer = 0f;
                Wandering();
                wanderCooldown = UnityEngine.Random.Range(1f, 3f); // 少し待ってから動く
            }
        }
        else
        {
            stuckTimer = 0f; // 動いてたらタイマーリセット
        }

        lastPosition = transform.position;
        return;
    }

    void ViewAction()
    {
        
        Vector3 viewPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        float halfAngle = viewAngle / 2f;

        bool hitTarget = false; // ← ループ外で初期化！

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 dir = rotation * transform.forward;

            Ray ray = new Ray(viewPos, dir);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player") && hit.collider.name == "body")
                {
                    // プレイヤーが見えてる
                    hitTarget = true;

                    lastTarget = hit.collider.transform;
                    isChasingPlayer = true;

                    Vector3 targetDir = lastTarget.position - transform.position;
                    targetDir.y = 0f;

                    if (targetDir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
                    }

                    float distance = Vector3.Distance(transform.position, lastTarget.position);
                    if (distance <= 1.5f)
                    {
                        Attack();
                    }

                    return; // プレイヤー見つけたら終了
                }
            }

            Debug.DrawRay(viewPos, dir * rayDistance, Color.red);
        }

        // 地面チェック（必要なら）
        if (!CheckGrounded())
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("run", false);
            isAttacking = false;
            isChasingPlayer = false;
            return;
        }

        if (!hitTarget)
        {
           
            //プレイヤーの痕跡を追跡
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

                
                // プレイヤーとの距離が一定以上離れたら追跡やめる
                float dist = Vector3.Distance(transform.position, lastTarget.position);
                if (dist > 5f)
                {
                    isChasingPlayer = false;
                    lastTarget = null;
                }
            }
            else
            {
                Wandering();
            }
        }

       // Stuck();

        


    }
    /// <summary>
    /// 地面の判定
    /// </summary>
    /// <returns></returns>
    private bool CheckGrounded()
    {
        Vector3 origin1 = (transform.position + transform.forward.normalized * 2f);
        Vector3 origin2 = (transform.position + transform.forward.normalized * 1.5f);
        Vector3 origin3 = (transform.position + transform.forward.normalized * 1f);
        Vector3 origin4 = (transform.position + transform.forward.normalized * 0.5f);
        Vector3 direction = Vector3.down;
        float rayLength = 2f;
        //int rayCount = 3;
        int groundLayer = LayerMask.GetMask("Ground");

        //for (int i = 0; i < rayCount; i++)
        //{
        //    Debug.DrawRay(origin, direction * rayLength, Color.red);
        //    return Physics.Raycast(origin , direction, rayLength, groundLayer);
        //}

        Debug.DrawRay(origin1, direction * rayLength, Color.red);
        Debug.DrawRay(origin2, direction * rayLength, Color.red);
        Debug.DrawRay(origin3, direction * rayLength, Color.red);

       return Physics.Raycast(origin1, direction, rayLength, groundLayer)
                && Physics.Raycast(origin2, direction, rayLength, groundLayer)
                    && Physics.Raycast(origin3, direction, rayLength, groundLayer) 
                        && Physics.Raycast(origin4, direction, rayLength, groundLayer);
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
        enemySpawn.enemyList.Remove(this);
    }
}
