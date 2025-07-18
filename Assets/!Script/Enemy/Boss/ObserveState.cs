using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を観察する（なんかかっこいいから！）
/// </summary>
/// <returns></returns>
public class ObserveState : BossState
{
    public override async UniTask Enter(Boss boss)
    {
        Debug.Log("様子見状態に入った");
        await UniTask.CompletedTask;
    }

    public override async UniTask Execute(Boss boss)
    {

        await UniTask.CompletedTask;
    }

    public override async UniTask Exit(Boss boss)
    {
        Debug.Log("様子見状態を抜けた");
        await UniTask.CompletedTask;
    }
}
