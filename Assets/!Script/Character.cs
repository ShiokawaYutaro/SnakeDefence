
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Rigidbody rb;

    //ステータス
    protected float HP;
    protected float MaxHp;
    protected float speed;
    protected float SlowSpeed;
    protected float NormalSpeed;
    protected float RunSpeed;
    protected float Stamina;
    protected float MaxStamina;

    protected Animator animator;
    protected Animator SkillAnim;
    protected Animator ModeAnim;
    protected Transform target;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HP = MaxHp;
        Stamina = MaxStamina;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
    }

}
