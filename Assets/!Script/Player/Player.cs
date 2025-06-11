using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : Character
{
    //=============================================================
    //          �}�l�[�W���[�̒�`
    //=============================================================
    [SerializeField] GameObject damageNotation;
    //public int Num;

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
    bool down;

    float animTime;

    Vector3 latestPos;

    Vector3 moveDirection = Vector3.zero;

    Vector3 CameraForward;
    Vector3 MoveForward;

    //public GameObject HPbar;
    public Image healthImage;
    float duration = 0.2f;
    float HcurrentRate = 1.0f;

    ////���񂾃p�[�e�B�N��
    //[SerializeField] ParticleSystem Exp;

    //�X�L�������t���O
    //UI-------------------------------------------------------------


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        MaxHp = 100f;
        speed = 10;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        //keyManager = GameObject.Find("KeyManager").GetComponent<KeyManager>();
        
        TDMove();

        if (Input.GetKey(KeyCode.P))
        {
            SetDamage(1f);
        }
        healthImage.transform.LookAt(Camera.main.transform.position);
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

    void OnTriggerStay(Collider other)
    {
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