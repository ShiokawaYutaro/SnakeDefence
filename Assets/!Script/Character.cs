
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Rigidbody rb;

    protected int LVL = 1;
    //ステータス
    public float HP { get; protected set; }
    public float MaxHp { get; set; }
    public float speed { get;  set; }
    public float power { get;  set; }
    public float defence { get;  set; }

    protected Animator animator;
    protected Animator SkillAnim;
    protected Animator ModeAnim;

    public bool attack;
    public bool isAttacking;
    protected bool playAnim;

    public const float _HEAL_AMOUNT = 10;

    // Start is called before the first frame update
    protected virtual void Start()
    {
       
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
    }

    public void OnAttack()
    {
        attack = true;
    }
    public void OffAttack()
    {
        attack = false;
    }
    public void OffIsAttacking()
    {
        isAttacking = false;
    }
    public void OnAnim()
    {
        playAnim = true;
    }
    public void OffAnim()
    {
        playAnim = false;
    }

    public void SetUp()
    {
        HP = MaxHp;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// power += value;
    /// </summary>
    /// <param name="value"></param>
    public void SetPower(float value) { power += value; }
    public float GetPower() { return power; }
    public void SetDefence(float value) { defence += value; }
    public float GetDefence() { return defence; }
    public void SetMaxHP(float value) { MaxHp += value; HP = MaxHp; }
    public float GetMaxHP() { return MaxHp; }

    /// <summary>
    /// アニメーションの終了待ち
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    protected async UniTask WaitUntilAnimationStateExits(string stateName)
    {
        // "Attack"ステートに入るまで待機
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            await UniTask.Yield();
        }

        // "Attack"ステートを抜けるまで待機
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            await UniTask.Yield();
        }
    }

}
