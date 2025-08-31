using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!player.attack) return;

            enemy.lastAttackPlayerName = player.transform.root.name;
            // 自分のColliderを取得（例えばアタック用のコライダー）
            Collider myCollider = GetComponent<Collider>();

            // 最近接点を計算
            Vector3 hitPoint = Physics.ClosestPoint(myCollider.bounds.center, other, other.transform.position, other.transform.rotation);

            if (player.ult)
            {
                enemy.SetDamage(player.power*2 + player.damageBonus);
                SEManager.Instance.PlayerULTAttackSE();
            }
            else
            {
                enemy.SetDamage(player.power + player.damageBonus);
                SEManager.Instance.PlayerAttackSE();
            }
            
            var effect = Instantiate(hitEffect, hitPoint, Quaternion.identity);
            Destroy(effect, 3);
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
