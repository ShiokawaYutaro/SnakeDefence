using Cysharp.Threading.Tasks;
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

        if (!targetEnemy.Contains(enemy))
        {
            targetEnemy.Add(enemy);
        }

        // �U�����łȂ���΁A�U��������񓯊��ŊJ�n
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
            // ���񂾓G�����O
            targetEnemy.RemoveAll(e => e == null || e.dead);

            if (targetEnemy.Count == 0) break;

            // �擪�̓G���U��
            GameObject target = targetEnemy[0].gameObject;

            // ���ۂ̍U�������i�A�j���[�V�������܂ށj��ҋ@
            player.Attack(target);

            // �C�ӂ̃N�[���^�C��
            await UniTask.Delay(1000); // 1�b�҂Ȃ�

            // �ēx���X�g���`�F�b�N
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