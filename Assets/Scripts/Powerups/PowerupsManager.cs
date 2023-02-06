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

    //Determine if a powerup will spawn or not
    public void CalculatePowerupChance(Vector3 position)
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= buffChance)
        {
            alreadySpawned = true;
            Powerup newBuff = GetCollectable(true, position);
        }

        if (debuffSpawnChance <= debuffChance && !alreadySpawned)
        {
            Powerup newDebuff = GetCollectable(false, position);
        }
    }

    //Returns a collectable and creates it in the game world
    private Powerup GetCollectable(bool isBuff, Vector3 position)
    {
        List<Powerup> powerup;

        if (isBuff)
        {
            powerup = AvailableBuffs;
        }
        else
        {
            powerup = AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, powerup.Count);
        Powerup prefab = powerup[buffIndex];
        Powerup newPowerup = Instantiate(prefab, position, Quaternion.identity) as Powerup;

        return newPowerup;
    }
}
