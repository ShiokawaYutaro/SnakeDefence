using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : Character
{
    //=============================================================
    //          �}�l�[�W���[�̒�`
    //=============================================================
    [SerializeField] GameObject damageNotation;

    bool down;
    

    //public GameObject HPbar;
    public Image healthImage;
    [SerializeField]Text healthText;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;
    public int poison;
    public int fire;
    public int regene;

    float maxLVLGauge = 2;
    float currentLVLGauge = 0;

    float healTime;

    [SerializeField] private Image attackArea;
    protected float attackRadious = 2;
    bool inArea;

    float attackInterval = 1;
    float attackTime;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private GameObject Gun;

    // Start is called before the first frame update
    protected override void Start()
    {
        MaxHp = 100f;
        speed = 10;
        damage = 1;
        attackArea.transform.localScale = new Vector3(attackRadious, attackRadious, attackRadious);
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        //keyManager = GameObject.Find("KeyManager").GetComponent<KeyManager>();
       

        healthImage.transform.LookAt(Camera.main.transform.position);

        Text text = GameObject.Find("lvlText").GetComponent<Text>();
        text.text = "Lv" + LVL.ToString();
        healthText.text = HP.ToString();

        healTime += Time.deltaTime;
        if (healTime > 10 && regene >= 1)
        {
            healTime = 0;
            SetHeal(10);
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

    public void Attack(GameObject target)
    {
        if (rb.velocity.magnitude > 0.01f) return;
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);
        attackTime += Time.deltaTime;
        if (attackTime > attackInterval)
        {
            attackTime = 0;
            Instantiate(bulletPrefab, Gun.transform.position, Gun.transform.rotation);
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
}