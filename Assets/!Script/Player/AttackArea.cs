using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private Player player;
    private List<Enemy> targetEnemy = new List<Enemy>();
    private bool isAttacking = false;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null || enemy.dead) return;

        // d•¡’Ç‰Á–hŽ~
        if (!targetEnemy.Contains(enemy))
        {
            targetEnemy.Add(enemy);
        }

        // ‚Ü‚¾UŒ‚’†‚Å‚È‚¢A‚©‚Â—LŒø‚È“G‚ª‚¢‚é
        if (!isAttacking && targetEnemy.Count > 0)
        {
            // æ“ª‚Ì“G‚ªŽ€‚ñ‚Å‚½‚çíœ
            while (targetEnemy.Count > 0 && (targetEnemy[0] == null || targetEnemy[0].dead))
            {
                targetEnemy.RemoveAt(0);
            }

            if (targetEnemy.Count > 0)
            {
                isAttacking = true;
                player.Attack(targetEnemy[0].gameObject);
                isAttacking = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && targetEnemy.Contains(enemy))
            {
                targetEnemy.Remove(enemy);
            }
        }
    }

}
