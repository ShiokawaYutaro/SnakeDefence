using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject hitEffect;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!player.attack) return;

            // ������Collider���擾�i�Ⴆ�΃A�^�b�N�p�̃R���C�_�[�j
            Collider myCollider = GetComponent<Collider>();

            // �ŋߐړ_���v�Z
            Vector3 hitPoint = Physics.ClosestPoint(myCollider.bounds.center, other, other.transform.position, other.transform.rotation);

            if (player.ult)
            {
                enemy.SetDamage(player.damage*2 + player.damageBonus);
            }
            else
            {
                enemy.SetDamage(player.damage + player.damageBonus);
            }
            
            Instantiate(hitEffect, hitPoint, Quaternion.identity);
           // TriggerShockwave(hitPoint, enemy.gameObject);
            player.ChargeReset();
        }
    }

    public void TriggerShockwave(Vector3 hitPos, GameObject hitObj)
    {
        Vector3 dir = (hitObj.transform.position - hitPos).normalized;
        hitObj.GetComponent<Rigidbody>().AddForce(dir * 3000, ForceMode.Impulse);
    }

    private void Update()
    {
    }
}
