using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sora : Player
{
    [SerializeField] ParticleSystem slashEffect;
    protected override void Start()
    {
        MaxHp = 100f;
        speed = 10;
        damage = 30;
        attackRadious = 1;
        attackInterval = 1;
        base.Start();
    }

    public override void Attack(GameObject target)
    {
        if (isAttacking) return;
        base.Attack(target);
        if (attackTime > attackInterval)
        {
            attackTime = 0;
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }

    }

    public void SlashEffect()
    {
        Instantiate(slashEffect,transform.position,Quaternion.Euler(15,50,180));
    }
}
