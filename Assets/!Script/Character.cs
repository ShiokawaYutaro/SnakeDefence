
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Rigidbody rb;

    protected int LVL = 1;
    //ステータス
    public float HP { get; protected set; }
    protected float MaxHp;
    public float speed { get; protected set; }
    public float damage { get; protected set; }

    protected Animator animator;
    protected Animator SkillAnim;
    protected Animator ModeAnim;

    public bool attack;
    public bool isAttacking;
    protected bool playAnim;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HP = MaxHp;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        animator.SetBool("attack", false);
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
