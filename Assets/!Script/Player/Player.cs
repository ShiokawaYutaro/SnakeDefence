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

    int LVL = 1;
    float maxLVLGauge = 2;
    float currentLVLGauge = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        MaxHp = 100f;
        speed = 10;
        damage = 1;

        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        //keyManager = GameObject.Find("KeyManager").GetComponent<KeyManager>();
        
        TDMove();

        healthImage.transform.LookAt(Camera.main.transform.position);

        Text text = GameObject.Find("lvlText").GetComponent<Text>();
        text.text = "Lv" + LVL.ToString();
        healthText.text = HP.ToString();
    }





    //�֐�===================================================================================
    public void TDMove()
    {
        //rb.AddForce(Vector3.down * 9.8f, ForceMode.Force);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 移動
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * speed;

        // 回転（移動方向があるときのみ）
        if (moveDir.magnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }


    }

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

        healthText.text = HP.ToString();

        GameObject damageText = Instantiate(damageNotation, transform.Find("ゲーム内/healthImage"));
        damageText.GetComponent<Text>().text = _damage.ToString();
        Destroy(damageText, 1);
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
        

        if (currentLVLGauge == maxLVLGauge) { LVLUP(); }
        gauge.DOFillAmount((currentLVLGauge + addGauge) / maxLVLGauge,duration);
        currentLVLGauge = currentLVLGauge + addGauge;

        Debug.Log(currentLVLGauge);
        
    }
    void LVLUP()
    {       
        LVL++;
        SkillCardManager.instance.DrawCard();
        currentLVLGauge -= maxLVLGauge;
        maxLVLGauge *= 1.5f;
        TailFollowManager.instance.AddTrail(3);
        MaxHp = MaxHp * LVL;
        HP = MaxHp;
        damage = damage * LVL;
        speed = speed + 0.01f * LVL;
    }
}