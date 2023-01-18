using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private SpriteRenderer sr;

    public ParticleSystem lightningBallEffect;

    public float lightningBallDuration = 10f;
    public bool isLightningBall;

    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningEnabled;
    public static event Action<Ball> OnLightningDisabled;

    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
    }

    //Destroys the ball if it falls into the death wall
    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    //Start lightning ball
    public void StartLightningBall()
    {
        if (!this.isLightningBall)
        {
            this.isLightningBall = true;
            this.sr.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBall(this.lightningBallDuration));

            OnLightningEnabled?.Invoke(this);
        }
    }

    //Stops lightning ball effect after tiem runs out
    private IEnumerator StopLightningBall(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StopLightningBall();
    }

    //Stops the lightning ball effect
    private void StopLightningBall()
    {
        if (this.isLightningBall)
        {
            this.isLightningBall = false;
            this.sr.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningDisabled?.Invoke(this);
        }
    }
}
