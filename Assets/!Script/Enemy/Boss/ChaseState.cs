using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossState
{
    public override async UniTask Enter(Boss boss)
    {
        Debug.Log("’Ç‚¤ó‘Ô‚É“ü‚Á‚½");
        await UniTask.CompletedTask;
    }

    public override async UniTask Execute(Boss boss)
    {
        Debug.Log("’Ç‚¢‚©‚¯’†");
        await boss.StartChase();
    }

    public override async UniTask Exit(Boss boss)
    {
        Debug.Log("’Ç‚¤ó‘Ô‚ğ”²‚¯‚½");
        await UniTask.CompletedTask;
    }
}
