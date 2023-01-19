using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton



    private static SoundManager _instance;

    public static SoundManager Instance => _instance;


    private void Awake()
    {
        //Ensures that there is only 1 instance of soundManager
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

    public AudioSource ballHit;
    public AudioSource brickDestroyed;

    public void PlayBallHit()
    {
        ballHit.Play();
    }

    public void PlayBrickDestroyed()
    {
        brickDestroyed.Play();
    }
}
