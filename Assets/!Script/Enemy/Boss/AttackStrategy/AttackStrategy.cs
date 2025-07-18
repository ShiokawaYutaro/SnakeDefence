using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AttackStrategy
{
    public abstract UniTask Execute(Boss boss);
}

public class GoingAttack : AttackStrategy
{
    public override async UniTask Execute(Boss boss)
    {
        Debug.Log("Ú‹ßUŒ‚");
        await boss.GoingAttack();
    }
}

public class LongRangeAttack : AttackStrategy
{
    public override async UniTask Execute(Boss boss)
    {
        Debug.Log("‰“‹——£UŒ‚");
        await boss.LongRangeAttack();
    }
}

public class CounterAttack : AttackStrategy
{
    public override async UniTask Execute(Boss boss)
    {
        Debug.Log("ƒJƒEƒ“ƒ^[UŒ‚");
        await boss.CounterAttack();
    }
}

/// <summary>
/// Œã‚ë‚É‰ñ”ğ‚µ‚ÄŠÔ‡‚¢‚ğæ‚é
/// </summary>
/// <returns></returns>
public class TakeDistanceState : AttackStrategy
{
    public override async UniTask Execute(Boss boss)
    {
        await boss.StartTakeDistance();
    }
}
