using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    private GameObject player;
    GameObject target;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    [SerializeField] private GameObject damageNotation;
    bool atkDelay;

    float viewAngle = 180f;
    int rayCount = 20;
    float rayDistance = 1.5f;


    public  bool attack;
    bool playAnim;
    public bool dead;

    protected override void Start()
    {
        target = GameObject.Find("家");
        MaxHp = 100f;
        speed = 3;
        damage = 10;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        healthImage.transform.LookAt(Camera.main.transform.position);
       // rb.velocity = transform.forward * speed;

        if(rb.velocity.magnitude >= 0.01f)
        {
            animator.SetBool("attack", false);
            animator.SetBool("run", true);
        }

        ViewAction();
    }

    void ViewAction()
    {
        Vector3 viewPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
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

                // プレイヤーを見つけた
                if (hit.collider.CompareTag("Player") && hit.collider.name == "body")
                {
                    rb.velocity = Vector3.zero;
                    animator.SetBool("run", false);
                    animator.SetBool("attack", true);
                    playAnim = true;
                    hitTarget = true;

                    // Y軸だけ回転
                    Vector3 targetDir = hit.collider.transform.position - transform.position;
                    targetDir.y = 0f;

                    if (targetDir != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                    }

                    break; // ← プレイヤーを見つけたらそれ以上チェックしない
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
        if (!hitTarget && !playAnim)
        {
            rb.velocity = transform.forward * speed;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.transform.position - transform.position),
                Time.deltaTime * 5f
            );

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
        HP -= _damage;
        float targetRate = HcurrentRate - _damage / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);

        GameObject damageText = Instantiate(damageNotation,transform.Find("UI/healthImage"));
        damageText.GetComponent<Text>().text = _damage.ToString();
        Destroy(damageText , 1);
        if(HP < 0)
        {
            animator.SetBool("dead", true);
            dead = true;
        }
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
}
