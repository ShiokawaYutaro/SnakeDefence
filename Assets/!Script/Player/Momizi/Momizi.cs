using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momizi : Player
{
    protected override void Start()
    {
        MaxHp = 100f;
        speed = 10;
        damage = 10;
        defence = 1;
        attackRadious = 1;
        attackInterval = 1;
        chargePower = 1;
        base.Start();
    }

    public override void Attack(GameObject target)
    {
        if (rb.velocity.magnitude > 0.01f) { return; }
        // if (isAttacking) return;
        base.Attack(target);
        if (attackTime > attackInterval)
        {
            attackTime = 0;
            
            animator.SetTrigger("attack");
        }

    }

    
}
