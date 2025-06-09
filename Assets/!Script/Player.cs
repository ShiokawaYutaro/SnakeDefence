using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : Character
{
    //=============================================================
    //          �}�l�[�W���[�̒�`
    //=============================================================
    [SerializeField] ItemManager itemManager;

    public int Num;

    ////�|�[�Y��ʗp
    //public GameObject pause;
    ////�|�[�Y��ʂ̌��݂̃��C���[
    //int currentLayer;
    //public GameObject Layer1;
    //public GameObject Layer2;
    //public GameObject Layer3;

    //[SerializeField] GameObject result;

    ////�|�����Ƃ��̃X�e�[�^�X�|�C���g
    //static public int StatusPoint;
    //static public int LVL = 1;
    //static public int LVLPoint;

    //PlayerAnimation


    float animTime;

    Vector3 latestPos;

    Vector3 moveDirection = Vector3.zero;

    Transform cameraPos;

    bool jump;
    bool sliding;
    bool run;

    bool squat;
    bool down;

    Vector3 CameraForward;
    Vector3 MoveForward;

    float ItemDis = 2;

    Transform Slot;
    int SlotCount;
    GameObject SelectMark;
    GameObject SetItemImage;
    GameObject PlayerHand;
    new camera camera;

    //public GameObject HPbar;
    public Image healthImage;
    public Image staminaImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;
    float ScurrentRate = 1.0f;

    protected float StaminaTime;
    protected float StaminaInterval;
    bool StaminaZero;


    public GameObject pickImage;  // PickImage（UIとして表示するもの）
    ////���񂾃p�[�e�B�N��
    //[SerializeField] ParticleSystem Exp;

    //�X�L�������t���O
    //UI-------------------------------------------------------------


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        NormalSpeed = 2;

        //Slot = transform.Find("ゲーム画面/インベントリ関連/アイテムスロット").transform;
        //SelectMark = transform.Find("ゲーム画面/インベントリ関連/選択マーク").gameObject;
        //PlayerHand = transform.Find("Camera/PlayerHand").gameObject;
        camera = GameObject.Find("Camera").GetComponent<camera>();

        cameraPos = camera.transform;

        if(Num == 1) {  }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        //keyManager = GameObject.Find("KeyManager").GetComponent<KeyManager>();
        
        TDMove();
        

        //TakeOutItemName();
        StatusMain();


        if (Input.GetKey(KeyCode.P))
        {
            SetDamage(1f);
        }

        //Debug.Log(time);
    }





    //�֐�===================================================================================
    public void TDMove()
    {
        rb.AddForce(Vector3.down * 9.8f, ForceMode.Force);

        if (!sliding)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 forward = cameraPos.forward;
            forward.y = 0f;
            CameraForward = forward.normalized;

            Vector3 right = cameraPos.right;
            right.y = 0f;

            MoveForward = CameraForward * moveZ + right.normalized * moveX;

            transform.rotation = Quaternion.AngleAxis(cameraPos.eulerAngles.y, Vector3.up);

            // 移動
            rb.velocity = MoveForward.normalized * speed + new Vector3(0, rb.velocity.y, 0);

        }

        if (rb.velocity.magnitude >= 0.01f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !sliding && Stamina > 0)
            {
                SetStaminaConsume();
                speed = RunSpeed;
                animator.SetBool("walk", false);
                animator.SetBool("run", true);

                run = true;
            }
            else
            {
                speed = NormalSpeed;
                animator.SetBool("run", false);
                animator.SetBool("walk", true);

                run = false;
            }
        }
        else
        {
            animator.SetBool("walk", false);
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            speed = SlowSpeed;
            squat = true;
            GetComponent<BoxCollider>().size = new Vector3(1f, 1, 1);
            GetComponent<BoxCollider>().center = new Vector3(0f, 0.5f, 0);
        }

        if (Input.GetKey(KeyCode.Space) && !jump)
        {
            jump = true;
            rb.velocity = Vector3.up * 5;
            Debug.Log("ジャンプした");
        }
        
        //if()

        //Debug.Log(rb.velocity.y);

        //if (sliding)
        //{
        //    rb.AddForce(rb.velocity * 2);
        //    animTime += Time.deltaTime;

        //}

    }

        
    //}

    //public void Door(KeyCode _keyCode)
    //{
    //    if(camera.HitItem != null)
    //    camera.HitItem.gameObject.GetComponent<Item>().Touching = false;
    //    if (Input.GetKey(_keyCode))
    //    {
    //        //扉開く
    //        if (camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("default") ||
    //            camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("閉じる"))
    //        {
    //            camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().SetBool("Open", true);
    //            camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().SetBool("Close", false);

    //        }
    //        //扉閉じる
    //        if (camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("開く"))
    //        {
    //            camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().SetBool("Close", true);
    //            camera.HitDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().SetBool("Open", false);
    //        }
    //    }


       

    //    //Debug.Log(camera.HItDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("default"));
    //    // Debug.Log(camera.HItDoor.transform.parent.parent.parent.parent.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
    //}


    //#region �퓬�@�^�C�v
    //void NormalSoot()
    //{
    //    bulletCount = bulletMax;
    //    Sniper = false;
    //    Sword = false;
    //    shotInterval = 0.1f;
    //    shotTime += Time.deltaTime;
    //    if (shotTime >= shotInterval && bulletCount >= bulletCurrent && !NormalReload)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {
    //            //Quaternion rot = Quaternion.Euler(0,0,-1);
    //            Instantiate(PlayerNormalBullet, transform.position, transform.rotation);
    //            shotTime = 0;
    //            bulletCurrent++;
    //        }
    //    }

    //    else if (bulletCurrent > bulletCount)
    //    {
    //        NormalReload = true;
    //    }

    //    if (Input.GetKey(KeyCode.R))
    //    {
    //        NormalReload = true;
    //    }

    //    if (NormalReload)
    //    {
    //        bulletCurrent = 0;
    //        bulletCount = 0;
    //        ReloadInterval = 3;
    //        ReloadTime += Time.deltaTime;
    //        ReloadUI.SetActive(true);
    //        front.DOFillAmount(ReloadTime / ReloadInterval, 0.5f);
    //        if (ReloadTime >= ReloadInterval)
    //        {
    //            bulletCurrent = 0;
    //            ReloadTime = 0;
    //            ReloadUI.SetActive(false);
    //            NormalReload = false;
    //        }
    //    }

    //}
    //void SniperSoot()
    //{
    //    SniperCount = bulletMax / 5;
    //    Sniper = true;
    //    Sword = false;
    //    shotInterval = 1f;
    //    shotTime += Time.deltaTime;
    //    if (shotTime >= shotInterval && SniperCount >= SniperCurrent && !SniperReload)
    //    {
    //        if (Input.GetMouseButton(0) && !UseSniperSKILL)
    //        {
    //            //Quaternion rot = Quaternion.Euler(0, 0, -1);
    //            Instantiate(PlayerSniperBullet, transform.position, transform.rotation);
    //            shotTime = 0;
    //            SniperCurrent++;
    //        }
    //    }

    //    else if (SniperCurrent > SniperCount)
    //    {
    //        SniperReload = true;
    //    }

    //    if (Input.GetKey(KeyCode.R))
    //    {
    //        SniperReload = true;
    //    }

    //    if (SniperReload)
    //    {
    //        ReloadInterval = 4;
    //        ReloadTime += Time.deltaTime;
    //        ReloadUI.SetActive(true);
    //        front.DOFillAmount(ReloadTime / ReloadInterval, 0.5f);
    //        if (ReloadTime >= ReloadInterval)
    //        {
    //            SniperCurrent = 0;
    //            ReloadTime = 0;
    //            ReloadUI.SetActive(false);
    //            SniperReload = false;
    //        }
    //    }



    //}

    //void SwordSoot()
    //{
    //    Sword = true;
    //    Sniper = false;
    //    if (Input.GetMouseButton(0)){ SwordDEF = false; SwordATK = true; }

    //    else {  SwordATK = false; }

    //    if (Input.GetMouseButton(1)){ SwordATK = false; SwordDEF = true; }

    //    else { SwordDEF = false; }

    //}
    //#endregion

    //#region �퓬�@�^�C�v�X�L��
    //void NormalSootSkill()
    //{

    //    IntervalSKILL = MaxIntervalNormalSKILL;
    //    //�m�[�}���X�L�����g���ĂȂ��Ȃ�X�L���Q�[�W���グ��
    //    if (!UseNormalSKILL) { NormalSKILL.value += Time.deltaTime * 0.03f *SKILLGaugeUp; Destroy(GameObject.Find("Funnel(Clone)")); }
    //    //�Q�[�W�}�b�N�X�� X�L�[ �������ƃX�L������
    //    if (NormalSKILL.value == 1 && Input.GetKeyDown(KeyCode.X)) { UseNormalSKILL = true; NormalSKILL.value = 0; }

    //    //�X�L�������Ȃ�
    //    if (UseNormalSKILL)
    //    {
    //        //�X�L������
    //        if (SKILLTime >= 0.1f && Input.GetKeyDown(KeyCode.X)){ SKILLTime = 20; }

    //        if (SKILLTime == 0)
    //        {
    //            Instantiate(FunnelPrefab, transform.position + transform.right * 2 ,transform.rotation,transform);
    //            Instantiate(FunnelPrefab, transform.position + transform.right * -2 ,transform.rotation,transform);
    //        }

    //        //�X�L�����ԏI��
    //        SKILLTime += Time.deltaTime;
    //        if(IntervalSKILL <= SKILLTime )
    //        {
    //            SKILLTime = 0;
    //            UseNormalSKILL = false;
    //        }
    //    }
    //}

    //void SniperSootSkill()
    //{
    //    IntervalSKILL = MaxIntervalSniperSKILL;
    //    //�X�i�C�p�[�X�L�����g���ĂȂ��Ȃ�X�L���Q�[�W���グ��
    //    if (!UseSniperSKILL) { SniperSKILL.value += Time.deltaTime * 0.03f * SKILLGaugeUp; Destroy(GameObject.Find("BimBullet(Clone)")); }
    //    //�Q�[�W�}�b�N�X�� X�L�[ �������ƃX�L������
    //    if (SniperSKILL.value == 1 && Input.GetKey(KeyCode.X)) { UseSniperSKILL = true; SniperSKILL.value = 0; }
    //    //�X�L�������Ȃ�
    //    if (UseSniperSKILL)
    //    {
    //        //�X�L������
    //        if (SKILLTime >= 0.1f && Input.GetKeyDown(KeyCode.X)) { SKILLTime = 20; }

    //        if (!BimSoot)
    //        {
    //            Instantiate(PlayerBimBullet, transform.position, transform.rotation);
    //            BimSoot = true;
    //        }

    //        //�X�L�����ԏI��
    //        SKILLTime += Time.deltaTime;
    //        if (IntervalSKILL <= SKILLTime )
    //        {
    //            SKILLTime = 0;
    //            UseSniperSKILL = false;
    //            BimSoot = false;
    //        }
    //    }
    //}

    //void SwordSootSkill()
    //{
    //    IntervalSKILL = MaxIntervalSwordSKILL;
    //    //�\�[�h�X�L�����g���ĂȂ��Ȃ�X�L���Q�[�W���グ��
    //    if (!UseSwordSKILL) { SwordSKILL.value += Time.deltaTime * 0.07f * SKILLGaugeUp; }
    //}
    //#endregion

    //�����蔻��


    void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.tag == "ClassroomDoornob")
        //{

        //}
        jump = false;
        //Debug.Log(jump);
    }

    //HP�o�[
    //�^�[�Q�b�g���b�N
    //void TargetRock()
    //{
    //    if (Input.GetKey(KeyCode.Q))
    //    {
    //        if (EnemyList.Count == 0)
    //        {
    //            return;
    //        }

    //        if (EnemyList.Count <= TargetCount)
    //        {
    //            TargetCount = 0;
    //        }
    //        target = EnemyList[TargetCount];
    //        TargetCount++;


    //    }

    //    if (target)
    //    {
    //        var pos = Vector3.zero;
    //        pos = target.transform.position;

    //    }

    //}
    //Enemy���b�N�I������


    //===========================================================================
    //���j���[�������悤
    // HP ゲージを更新する共通メソッド
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
    }

    public void SetStaminaConsume()
    {
        Stamina -= 0.1f;
        float targetRate = ScurrentRate - 0.1f / MaxStamina;
        UpdateFillAmount(staminaImage, ref ScurrentRate, targetRate, duration);
    }

    void SetStaminaRecovery()
    {
        Stamina += 0.1f;
        float targetRate = ScurrentRate + 0.1f / MaxStamina;
        UpdateFillAmount(staminaImage, ref ScurrentRate, targetRate, duration);
    }


#if false
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "main")
        {
            if (!GameResultSet)
            {
                if (currentLayer == 0)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        pause.SetActive(true);

                        currentLayer = 1;
                    }
                }
                else if (currentLayer >= 2)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentLayer = 1;
                    }
                }
                else if (currentLayer == 1)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentLayer = 0;
                    }
                }


                PauseGame();
            }

            if (GameResultSet)
            {
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.None;
                //Time.timeScale = 0;

            }
        }

        if(SceneManager.GetActiveScene().name == "Result")
        {
            
        }

        if (SceneManager.GetActiveScene().name == "Load")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
        }

    }


    void PauseGame()
    {
        //���C�����
        if (currentLayer == 0)
        {
            Time.timeScale = 1;
            pause.SetActive(false);
            GameMainPrint();
        }

        //�X�e�[�^�X���
        if (currentLayer == 1)
        {
            Layer1.SetActive(true);
            Layer2.SetActive(false);
            Layer3.SetActive(false);
            //�����֐�
            StatusPrint();
        }

        //�ݒ���
        if (currentLayer == 2)
        {
            Layer1.SetActive(false);
            Layer2.SetActive(true);
            Layer3.SetActive(false);
        }

        if (currentLayer == 3)
        {
            Layer1.SetActive(false);
            Layer2.SetActive(false);
            Layer3.SetActive(true);
        }

        

    }
#endif

    //ステータス関係------------------------------------------------------------------
    void StatusMain()
    {
        StaminaRelation();
    } 

    void Dead()
    {
        if(HP < 0)
        {
            HP = 0;
            down = true;
        }
    }

    void StaminaRelation()
    {
        if (!run)
        {

            if (StaminaZero)
            {
                StaminaTime += Time.deltaTime;
                if (StaminaTime > StaminaInterval * 2)
                {
                    SetStaminaRecovery();
                }
            }

            if (!StaminaZero)
            {
                StaminaTime += Time.deltaTime;
                if (StaminaTime > StaminaInterval)
                {
                    SetStaminaRecovery();
                }
            }
        }
        else
        {
            StaminaTime = 0;
            StaminaZero = false;
        }

        if(Stamina >= MaxStamina)
        {
            Stamina = MaxStamina;
        }

        if(Stamina <= -1)
        {
            Stamina = 0;
        }

        if (Stamina <= 0)
        {
            StaminaZero = true;

        }
    }
    //public void System() { currentLayer = 2; }
    //public void Explain() { currentLayer = 3; }
    //public void SpeedUP() 
    //{ 
    //    if( StatusPoint >= 1)
    //    {
    //        StatusPoint -= 1;
    //        MaxSpeed += 1; 
    //    }
    //}
    //public void HpUP() 
    //{
    //    if(StatusPoint >= 2)
    //    {
    //        StatusPoint -= 2;
    //        MaxHp += 10;
    //        HP += 10;
    //        SetGaugeHpUpPlus();
    //    }
    //}
    //public void RegeneUP()
    //{
    //    if (StatusPoint >= 1)
    //    {
    //        StatusPoint -= 1;
    //        Regene += 1;
    //    }
    //}
    //public void NormalUP()
    //{
    //    if (StatusPoint >= 2)
    //    {
    //        StatusPoint -= 2;
    //        MaxIntervalNormalSKILL += 1;
    //    }
    //}
    //public void SniperUP()
    //{
    //    if (StatusPoint >= 2)
    //    {
    //        StatusPoint -= 2;
    //        MaxIntervalSniperSKILL += 1;
    //    }
    //}
    //public void SwordUP()
    //{
    //    if (StatusPoint >= 2)
    //    {
    //        StatusPoint -= 2;
    //        MaxIntervalSwordSKILL += 1;
    //    }
    //}
    //public void SKILLGaugeUP()
    //{
    //    if (StatusPoint >= 5)
    //    {
    //        StatusPoint -= 5;
    //        SKILLGaugeUp += 1;
    //    }
    //}


    //UI関係------------------------------------------------------------------------
   

    ////�Q�[���I���֌W====================================================================================
    //public void GameResult()
    //{
    //    GameResultSet = true;
    //    result.SetActive(true);

    //    Debug.Log(Cursor.visible);

    //    Text KillCount = GameObject.Find("���j��").GetComponent<Text>();
    //    KillCount.text = "���j��\n" + Enemy.EnemyDethCout.ToString();

    //    Text ClearTime = GameObject.Find("�N���A����").GetComponent<Text>();
    //    ClearTime.text = "�N���A����\n" + (300 - Boss.EndTime).ToString("0");
    //}

    //public void ReStart()
    //{
    //    SceneManager.LoadScene("Load");
    //}

    //public void GameStart()
    //{
    //    SceneManager.LoadScene("main");
    //}
    
    //public void LoadFlash()
    //{
    //    flash = GameObject.Find("�t���b�V��").GetComponent<Image>();
    //    flash.DOFade(1.0f, 1.0f);
    //}
    //public void GameStop()
    //{
    //   // UnityEditor.EditorApplication.isPlaying = false;
    //}
}