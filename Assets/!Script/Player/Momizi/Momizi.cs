using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Momizi : Player
{
    [SerializeField] GameObject[] effects;
    [SerializeField] ParticleSystem ultEffect;

    protected override void Start()
    {
        //CoinManager.instance.AddCoin(2000);
        MaxHp = 100f;
        speed = 10;
        power = 10;
        defence = 1;
        attackRadious = 1;
        attackInterval = 1;
        chargePower = 1;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].transform.localScale = new Vector3(chargeImage.fillAmount, chargeImage.fillAmount, chargeImage.fillAmount);
        }
        
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

    public void ULTEffect()
    {
        var effect = Instantiate(ultEffect,transform);
        Destroy(effect,3);
    }

    
}
