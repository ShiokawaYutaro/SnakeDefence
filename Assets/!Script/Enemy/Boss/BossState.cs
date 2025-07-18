using Cysharp.Threading.Tasks;

//interface ってのがあるらしい

public abstract class BossState
{
    /// <summary>
    /// ステートに入る
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public abstract UniTask Enter(Boss boss);
    /// <summary>
    /// ステートをじっこうする
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public abstract UniTask Execute(Boss boss);
    /// <summary>
    /// ステートから出る
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public abstract UniTask Exit(Boss boss);
}
