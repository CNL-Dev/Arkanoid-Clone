using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

    private void Awake()
    {
        //Ensures that there is only 1 instance of ballsManager
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

    [SerializeField]
    private Ball ballPrefab;
    private Ball initialBall;
    private Rigidbody2D initialBallRb;
    public float initialBallSpeed = 250f;

    public List<Ball> Balls { get; set; }

    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            //Aligne the ball to the paddle position
            Vector3 paddlePos = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPos = new Vector3(paddlePos.x, paddlePos.y + 0.27f, 0f);
            initialBall.transform.position = ballPos;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRb.isKinematic = false;
                initialBallRb.AddForce(new Vector2(0f, initialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    private void InitBall()
    {
        Vector3 paddlePos = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPos = new Vector3(paddlePos.x, paddlePos.y + 0.27f, 0f);
        initialBall = Instantiate(ballPrefab, startingPos, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }

    public void ResetBalls()
    {
        //Destroy any balls that are in the scene
        foreach(var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
        //Create a new ball
        InitBall();
    }

    public void SpawnBalls(Vector3 position, int count, bool isLightningBall)
    {
        for(int i = 0; i < count; i++)
        {
            Ball spawnedBall = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;
            if (isLightningBall)
            {
                spawnedBall.StartLightningBall();
            }

            Rigidbody2D spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
            spawnedBallRb.isKinematic = false;
            spawnedBallRb.AddForce(new Vector2(0f, initialBallSpeed));
            this.Balls.Add(spawnedBall);
        }
    }
}
