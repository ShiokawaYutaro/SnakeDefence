using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momizi : Player
{
    protected override void Start()
    {
        attackRadious = 1;
        base.Start();
    }

    public override void Attack(GameObject target)
    {
        base.Attack(target);
        animator.SetBool("attack", true);
        attackTime += Time.deltaTime;
        if (attackTime > attackInterval)
        {
            attackTime = 0;
        }
    }
}
