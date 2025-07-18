using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 待機状態になって休憩するようにしたいなぁ（モウハンみたいに）
/// </summary>
/// <returns></returns>
public class IdelState : BossState
{
    public override async UniTask Enter(Boss boss)
    {
        Debug.Log("待機状態に入った");
        await UniTask.CompletedTask;
    }

    public override async UniTask Execute(Boss boss)
    {
        await UniTask.CompletedTask;
    }

    public override async UniTask Exit(Boss boss)
    {
        Debug.Log("待機状態を抜けた");
        await UniTask.CompletedTask;
    }
}
