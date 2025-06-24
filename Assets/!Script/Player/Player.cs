using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class Player : Character
{
    //=============================================================
    //          �}�l�[�W���[�̒�`
    //=============================================================
    [SerializeField] GameObject damageNotation;

    bool down;
    

    //public GameObject HPbar;
    public GameObject healthBar;
    public GameObject chargeBar;
    Image healthImage;
    protected Image chargeImage;
    [SerializeField]Text healthText;
    protected float chargePower;
    public float damageBonus;

    float duration = 0.2f;
    float HcurrentRate = 1.0f;
    public int poison;
    public int fire;
    public int regene;

    float maxLVLGauge = 2;
    float currentLVLGauge = 0;

    float healTime;

    [SerializeField] private Image attackArea;
    protected float attackRadious = 1;
    bool inArea;

    protected float attackInterval = 1;
    protected float attackTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        attackArea.transform.localScale = new Vector3(attackRadious, attackRadious, attackRadious);
        base.Start();

        healthImage = healthBar.transform.Find("front").GetComponent<Image>();
        chargeImage = chargeBar.transform.Find("front").GetComponent<Image>();

        chargeImage.fillAmount = 0;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        //keyManager = GameObject.Find("KeyManager").GetComponent<KeyManager>();
       

        healthBar.transform.LookAt(Camera.main.transform.position);

        if (!isAttacking)
        {
            chargeImage.fillAmount += chargePower * 0.001f;
            damageBonus = chargeImage.fillAmount * damage;
            Debug.Log(damageBonus);
        }


        Text text = GameObject.Find("lvlText").GetComponent<Text>();
        text.text = "Lv" + LVL.ToString();
        healthText.text = HP.ToString();

        healTime += Time.deltaTime;
        if (healTime > 10 && regene >= 1)
        {
            healTime = 0;
            SetHeal(10);
        }

        attackTime += Time.deltaTime;


        // 現在のゲームパッド情報
        var current = Gamepad.current;

        // ゲームパッド接続チェック
        if (current == null)
            return;

        // 左スティック入力取得
        var leftStickValue = current.leftStick.ReadValue();
        float moveX = leftStickValue.x;
        float moveZ = leftStickValue.y;

        // 移動
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * speed;

        // 回転（移動方向があるときのみ）
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
            animator.SetBool("attack", false);
            animator.SetBool("run", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("run", false);
        }
    }





    //�֐�===================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "ピッケル")
        {
            Enemy enemy = other.transform.root.GetComponent<Enemy>();
            if (enemy.attack)
            {
                SetDamage(enemy.damage);
                enemy.attack = false;
            }
            
        }
    }

    public virtual void Attack(GameObject target)
    {
        Vector3 targetDir = target.transform.position;
        targetDir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);
        transform.DOLookAt(targetDir,1);
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

        healthText.text = HP.ToString("f0");

        GameObject damageText = Instantiate(damageNotation, transform.Find("ゲーム内/healthImage"));
        damageText.GetComponent<Text>().text = _damage.ToString("f1");
        Destroy(damageText, 1);
    }

    public void SetHeal(float _addHeal)
    {
        HP += _addHeal;
        UpdateFillAmount(healthImage, ref HcurrentRate, _addHeal, duration);
        if(HP > MaxHp)
        {
            HcurrentRate = 1;
            HP = MaxHp;
        }

    }

    //ステータス関係------------------------------------------------------------------
    void Dead()
    {
        if(HP < 0)
        {
            HP = 0;
            down = true;
        }
    }
    public void LVLGauge(float addGauge)
    {
        Image gauge = GameObject.Find("lvlGauge").GetComponent<Image>();

        currentLVLGauge += addGauge;
        if (currentLVLGauge >= maxLVLGauge) { LVLUP(); }
        gauge.DOFillAmount(currentLVLGauge / maxLVLGauge,duration);
        
    }
    public virtual void LVLUP()
    {       
        LVL++;
        SkillCardManager.instance.StartDraw();
        currentLVLGauge -= maxLVLGauge;
        maxLVLGauge *= 1.5f;
        float upStatus =(float) LVL * 0.2f + 1;
        MaxHp = MaxHp * upStatus;
        //HP = MaxHp;
        //UpdateFillAmount(healthImage, ref HcurrentRate, HP, duration);
        //damage = damage * upStatus;
        speed = speed + 0.01f * upStatus;
    }

    public void ChargeReset()
    {
        chargeImage.fillAmount = 0;
        damageBonus = 0;
    }
}