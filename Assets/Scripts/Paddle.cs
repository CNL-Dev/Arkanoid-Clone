using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    public bool PaddleIsTransforming { get; set; }

    private void Awake()
    {
        //Ensures that there is only 1 insatnce of gameManager
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixels = 200;
    private float defaultLeftClamp = 135;
    private float defaultRightClamp = 410;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public float extendShrinkDuration = 10f;
    public float paddleWidth = 2f;
    public float paddleHeight = 0.28f;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        PaddleMovement();
    }

    //Handle paddle movement via the users mouse
    private void PaddleMovement()
    {
        //Clamps the paddle to the camera
        //Adjusts camera clamp for the paddle
        float paddleShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePosXPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePosWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePosXPixels, 0f, 0f)).x;
        this.transform.position = new Vector3(mousePosWorldX, paddleInitialY, 0f);
    }

    //Bounces the ball depending on the part of the paddile it collides with
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0f);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if(hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            SoundManager.Instance.PlayBallHit();
        }
    }

    //Start the paddle width animation
    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }

    //Expands and shrinks the paddle when the appropiate powerup is obtained
    private IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        this.StartCoroutine(ResetPaddleWidth(this.extendShrinkDuration));

        if (width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;
            while(currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        this.PaddleIsTransforming = false;
    }

    //Resets the paddle width
    private IEnumerator ResetPaddleWidth(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }
}
