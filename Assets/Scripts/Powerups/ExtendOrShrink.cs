public class ExtendOrShrink : Powerup
{
    public float newWidth = 2.5f;

    protected override void ApplyEffect()
    {
        if(Paddle.Instance != null && !Paddle.Instance.PaddleIsTransforming)
        {
            Paddle.Instance.StartWidthAnimation(newWidth);
        }
    }
}