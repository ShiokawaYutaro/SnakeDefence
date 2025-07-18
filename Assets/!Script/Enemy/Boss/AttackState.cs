using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackState : BossState
{
  
    public　override async UniTask Enter(Boss boss)
    {
        Debug.Log("攻撃状態に入った");
        await UniTask.CompletedTask;
    }

    public override async UniTask Execute(Boss boss)
    {
        // 攻撃のロジック実行
        await boss.StartAttack(Random.Range((int)AttackType.Going, (int)AttackType.Max));
    }

    public override async UniTask Exit(Boss boss)
    {
        Debug.Log("攻撃状態を抜けた");
        await UniTask.CompletedTask;
    }
}

