using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ICollectible
{

    protected bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //destroy the collectible once it reaches the player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
