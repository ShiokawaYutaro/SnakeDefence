using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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

        if (!targetEnemy.Contains(enemy))
        {
            targetEnemy.Add(enemy);
        }

        // 攻撃中でなければ、攻撃処理を非同期で開始
        if (!isAttacking)
        {
            StartAttackLoop().Forget();
        }

    }

    private async UniTaskVoid StartAttackLoop()
    {
        isAttacking = true;

        while (targetEnemy.Count > 0)
        {
            // 死んだ敵を除外
            targetEnemy.RemoveAll(e => e == null || e.dead);

            if (targetEnemy.Count == 0) break;

            // 先頭の敵を攻撃
            GameObject target = targetEnemy[0].gameObject;

            // 実際の攻撃処理（アニメーション等含む）を待機
            player.Attack(target);

            // 任意のクールタイム
            await UniTask.Delay(1000); // 1秒待つなど

            // 再度リストをチェック
            targetEnemy.RemoveAll(e => e == null || e.dead);
        }

        isAttacking = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                targetEnemy.Remove(enemy);
            }
        }

    }
}