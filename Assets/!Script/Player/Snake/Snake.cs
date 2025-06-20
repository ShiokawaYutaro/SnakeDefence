
public class Snake : Player
{
    public override void LVLUP()
    {
        attackRadious = 0;
        base.LVLUP();
        TailFollowManager.instance.AddTrail(3);
    }
}
