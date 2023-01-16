using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsManager : MonoBehaviour
{
    #region Singleton



    private static PowerupsManager _instance;

    public static PowerupsManager Instance => _instance;


    private void Awake()
    {
        //Ensures that there is only 1 instance of powerupsManager
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

    public List<Powerup> AvailableBuffs;
    public List<Powerup> AvailableDebuffs;

    [Range(0, 100)]
    public float buffChance;

    [Range(0, 100)]
    public float debuffChance;
}
