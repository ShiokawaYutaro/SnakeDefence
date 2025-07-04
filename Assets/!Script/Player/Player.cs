using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

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

    public bool ult;
    int _fingerId = -1;
    [SerializeField] Transform ultCamera;


    public RectTransform joystickBG;
    public RectTransform joystickKnob;
    private float joystickRange = 100f;

    private int touchId = -1;
    private Vector2 startTouchPos;
    private bool isMoving = false;

    private bool isGrounded;

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
       
        if(HP <= 0)
        {
            HP = 0;
        }
        healthBar.transform.LookAt(Camera.main.transform.position);
        healthText.text = HP.ToString("f0") + "/" + MaxHp.ToString("f0");
        if (!isAttacking)
        {
            chargeImage.fillAmount += chargePower * 0.001f;
            damageBonus = chargeImage.fillAmount * damage;
           // Debug.Log(damageBonus);
        }


        Text text = GameObject.Find("lvlText").GetComponent<Text>();
        text.text = "Lv" + LVL.ToString();

        if(HP < MaxHp && regene >= 1)
        {
            healTime += Time.deltaTime;
            if (healTime > 10)
            {
                healTime = 0;
                SetHeal(_HEAL_AMOUNT * regene);
            }
        }
        

        attackTime += Time.deltaTime;


        if (ult)
        {
            Camera.main.transform.position = new Vector3(ultCamera.position.x, ultCamera.position.y, ultCamera.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(ultCamera.eulerAngles);
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
        }
        else
        {
            
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z - 6);
           // Camera.main.transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(50, 0, 0));
           Camera.main.transform.rotation = Quaternion.Euler(50, 0, 0);
            StickMove();
        }

       


    }



    private void StickMove()
    {
        float posY = rb.velocity.y;
        posY -= 0.1f;
        //// 現在のゲームパッド情報
        //var current = Gamepad.current;

        //// ゲームパッド接続チェック
        //if (current == null)
        //    return;

        //// 左スティック入力取得
        //var leftStickValue = current.leftStick.ReadValue();
        //float moveX = leftStickValue.x;
        //float moveZ = leftStickValue.y;

        //// 移動
        //Vector3 PCmoveDir = new Vector3(moveX, 0f, moveZ).normalized;
       
        //rb.velocity = new Vector3(PCmoveDir.x, posY, PCmoveDir.z) * speed;

        //// 回転（移動方向があるときのみ）
        //if (PCmoveDir.magnitude > 0.01f)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(PCmoveDir);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        //    animator.SetBool("run", true);
        //}
        //else
        //{
        //    rb.velocity = Vector3.zero;
        //    animator.SetBool("run", false);
        //}
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // タッチ開始
                if (touch.phase == UnityEngine.TouchPhase.Began && touchId == -1)
                {
                    touchId = touch.fingerId;
                    startTouchPos = touch.position;

                    joystickBG.gameObject.SetActive(true);
                    joystickKnob.gameObject.SetActive(true);
                    joystickBG.position = startTouchPos;
                    joystickKnob.position = startTouchPos;
                }

                // 該当指の移動
                if (touch.fingerId == touchId)
                {
                    if (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary)
                    {
                        Vector2 offset = touch.position - startTouchPos;
                        Vector2 clampedOffset = Vector2.ClampMagnitude(offset, joystickRange);
                        joystickKnob.position = startTouchPos + clampedOffset;

                        Vector3 moveDir = new Vector3(clampedOffset.x, 0, clampedOffset.y).normalized;
                        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * speed;

                        if (moveDir.magnitude > 0.01f)
                        {
                            Quaternion toRotation = Quaternion.LookRotation(moveDir);
                            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
                            animator.SetBool("run", true);
                        }

                        Vector3 origin = transform.position + moveDir / 3;
                        Vector3 direction = Vector3.down;

                        // デバッグ用にレイを可視化（Sceneビューで確認可能）
                        Debug.DrawRay(origin, direction * 2, Color.red);
                        int groundLayer = LayerMask.GetMask("Ground");
                        // 真下にレイを飛ばす
                        isGrounded = Physics.Raycast(transform.position + moveDir / 3, Vector3.down, 2, groundLayer);

                        if (!isGrounded)
                        {
                            // 地面がない → 移動禁止
                            rb.velocity = Vector3.zero;
                            continue;
                        }

                        isMoving = true;
                    }

                    if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
                    {
                        touchId = -1;
                        rb.velocity = Vector3.zero;
                        animator.SetBool("run", false);
                        joystickBG.gameObject.SetActive(false);
                        joystickKnob.gameObject.SetActive(false);
                        isMoving = false;
                    }
                }
            }
        }
        else if (isMoving)
        {
            // 指が離された場合に停止（保険）
            rb.velocity = Vector3.zero;
            animator.SetBool("run", false);
            joystickBG.gameObject.SetActive(false);
            joystickKnob.gameObject.SetActive(false);
            touchId = -1;
            isMoving = false;
        }

       
    }

    //�֐�===================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "武器")
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
        _damage -= defence;
        HP -= _damage;
        float targetRate = HcurrentRate - _damage / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);

       

        //GameObject damageText = Instantiate(damageNotation, transform.Find("ゲーム内/healthImage"));
        //damageText.GetComponent<Text>().text = _damage.ToString("f1");
        //Destroy(damageText, 1);
    }

    public void SetHeal(float _addHeal)
    {
        float actualHeal = Mathf.Min(_addHeal, MaxHp - HP); // 超えない分だけヒール
        HP += actualHeal;
        float targetRate = HcurrentRate + actualHeal / MaxHp;
        UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
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
        if (currentLVLGauge >= maxLVLGauge)
        {
            LVLUP(); // MaxHp が増える処理
                     // 回復はしないけど、FillAmount を現在HPに合わせて更新する
            float targetRate = HP / MaxHp;
            UpdateFillAmount(healthImage, ref HcurrentRate, targetRate, duration);
        }

        gauge.DOFillAmount(currentLVLGauge / maxLVLGauge, duration);
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

    public void UseUlt()
    {
        ult = true;
        animator.SetTrigger("ult");
    }
    public void UnUseUlt()
    {
        ult = false;
       
    }

}