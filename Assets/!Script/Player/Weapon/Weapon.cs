using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!player.attack) return;
            enemy.SetDamage(player.damage);
        }
    }
}
