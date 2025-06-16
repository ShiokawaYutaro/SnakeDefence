
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Rigidbody rb;

    //ステータス
    public float HP { get; protected set; }
    protected float MaxHp;
    protected float speed;
    public float damage { get; protected set; }

    protected Animator animator;
    protected Animator SkillAnim;
    protected Animator ModeAnim;

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

}
