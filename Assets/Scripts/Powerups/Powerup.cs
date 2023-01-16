using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class for powerups 
public abstract class Powerup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Paddle")
        {
            this.ApplyEffect();
        }

        if(collision.tag == "Paddle" || collision.tag == "DeathCollider")
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void ApplyEffect();
}
