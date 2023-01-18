
public class LightningBall : Powerup
{
    protected override void ApplyEffect()
    {
        foreach (var ball in BallsManager.Instance.Balls)
        {
            ball.StartLightningBall();
        }
    }
}
