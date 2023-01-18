using System.Linq;

public class MultiBall : Powerup
{
    //Generate more balls on pickup
    protected override void ApplyEffect()
    {
        foreach(Ball ball in BallsManager.Instance.Balls.ToList())
        {
            BallsManager.Instance.SpawnBalls(ball.gameObject.transform.position, 2, ball.isLightningBall);
        }
    }
}