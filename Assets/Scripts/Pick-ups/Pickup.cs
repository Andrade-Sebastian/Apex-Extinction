using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //destroy the collectible once it reaches the player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
