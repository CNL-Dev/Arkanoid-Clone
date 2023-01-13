using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;

    public int health = 1;
    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Start()
    {
        this.sr =this.GetComponent<SpriteRenderer>();
        this.sr.sprite = BricksManager.Instance.Sprites[this.health - 1]; //TODO: Replace and delete this line in favor of a Init method
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.health--;

        //Destroy brick if its health reaches 0
        if (this.health <= 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            //Change the sprite
            this.sr.sprite = BricksManager.Instance.Sprites[this.health - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
}
