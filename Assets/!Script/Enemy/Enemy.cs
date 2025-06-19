using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    private Player player;
    GameObject target;
    GameObject wayPoint1;
    GameObject wayPoint2;
    bool onWayPoint1;
    bool onWayPoint2;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;
    bool atkDelay;

    float viewAngle = 360;
    int rayCount = 20;
    float rayDistance = 1.5f;


    public  bool attack;
    bool playAnim;
    public bool dead;

    private Transform lastTarget = null;
    private bool isChasingPlayer = false;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = GameObject.Find("家");
        MaxHp = Random.Range(10,30) * LVL;
        
        damage = 10;
        base.Start();
        wayPoint1 = GameObject.Find("WayPoint1");
        wayPoint2 = GameObject.Find("WayPoint2");
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
        }

        ViewAction();
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

            if (Physics.Raycast(viewPos, dir, out RaycastHit hit, rayDistance))
            {

                if (hit.collider.CompareTag("Player") && hit.collider.name == "body")
                {
                    rb.velocity = Vector3.zero;
                    animator.SetBool("run", false);
                    animator.SetBool("attack", true);
                    playAnim = true;
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

                    break;
                }


                // 家を見つけた
                else if (hit.collider.transform.parent != null && hit.collider.transform.parent.gameObject == target)
                {
                    rb.velocity = Vector3.zero;
                    animator.SetBool("run", false);
                    animator.SetBool("attack", true);
                    playAnim = true;
                    hitTarget = true;

                    Vector3 targetDir = hit.collider.transform.position - transform.position;
                    targetDir.y = 0f;

                    if (targetDir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                    }

                    break;
                }
            }

            Debug.DrawRay(viewPos, dir * rayDistance, Color.red);
        }

        // 攻撃対象が見つからなかった場合、移動
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

            if (!onWayPoint1)
            {
                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wayPoint1.transform.position - transform.position),
                Time.deltaTime * 5f
                );
            }
            if (onWayPoint1)
            {
                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wayPoint2.transform.position - transform.position),
                Time.deltaTime * 5f
                );
            }
            if (onWayPoint2)
            {
                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.transform.position - transform.position),
                Time.deltaTime * 5f
                );
            }


            animator.SetBool("run", true);
            animator.SetBool("attack", false);
        }

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
        damageText.GetComponent<Text>().text = _damage.ToString();
        Destroy(damageText, 1);

    }

    public async void SetAttributeDamage(float _damage, int attribute , Color32 color)
    {
        if (dead) return;
        for (int i = 0; i < attribute; i++)
        {
            HP -= _damage;
            float targetRate = HcurrentRate - _damage / MaxHp;
            UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
            GameObject damageText = Instantiate(damageNotation, transform.Find("UI/healthImage"));
            damageText.GetComponent<Text>().color = color;
            damageText.GetComponent<Text>().text = _damage.ToString();
            Destroy(damageText, 1);
            await UniTask.Delay(2000);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "WayPoint1") { onWayPoint1 = true; }
        if(other.gameObject.name == "WayPoint2") { onWayPoint2 = true; }
    }
    public void OnAttack()
    {
        attack = true;
    }
    public void OffAttack()
    {
        attack = false;
    }
    public void OnAnim()
    {
        playAnim = true;
    }
    public void OffAnim()
    {
        playAnim = false;
    }
    public void Dead()
    {
        player.LVLGauge(1);
        Destroy(gameObject, 2);
    }
}
