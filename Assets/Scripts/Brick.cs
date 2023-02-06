using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public int health = 1;
    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCol = this.GetComponent<BoxCollider2D>();
        Ball.OnLightningEnabled += OnLightningEnabled;
        Ball.OnLightningDisabled += OnLightningDisabled;
    }

    private void OnLightningEnabled(Ball obj)
    {
        if(this != null)
        {
            this.boxCol.isTrigger = true;
        }
    }

    private void OnLightningDisabled(Ball obj)
    {
        if (this != null)
        {
            this.boxCol.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision object is a ball, apply logic
        if(collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ApplyCollisionLogic(ball);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If collision object is a ball, apply logic
        if (collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ApplyCollisionLogic(ball);
        }      
    }

    //When a ball collides with a brick, damage the brick
    private void ApplyCollisionLogic(Ball ball)
    {
        this.health--;

        //Destroy brick if its health reaches 0 or we have a lightning ball
        if (this.health <= 0 || (ball != null && ball.isLightningBall))
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
            SoundManager.Instance.PlayBrickDestroyed();
        }
        else
        {
            //Change the sprite
            this.sr.sprite = BricksManager.Instance.Sprites[this.health - 1];
            SoundManager.Instance.PlayBallHit();
        }
        
    }

    //Determines if a powerup will spawn after brick is destroyed
    private void OnBrickDestroy()
    {
        PowerupsManager.Instance.CalculatePowerupChance(this.transform.position);
    }

    //Spawns the particle effect after a brick is destroyed
    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    //Create a brick
    public void Init(Transform containerTransform, Sprite sprite, Color color, int health)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.health = health;
    }

    private void OnDisable()
    {
        Ball.OnLightningEnabled -= OnLightningEnabled;
        Ball.OnLightningDisabled -= OnLightningDisabled;
    }
}
