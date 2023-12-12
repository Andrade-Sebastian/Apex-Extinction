using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public float throwForce = 10f;
    public float throwCooldown = 2f;

    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;

        // Start throwing bombs
        InvokeRepeating("ThrowBomb", 0f, throwCooldown);
    }

    void ThrowBomb()
    {
        // Instantiate a bomb
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        // Calculate the throw direction
        Vector2 throwDirection = (player.position - transform.position).normalized;

        // Apply force to the bomb
        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        bombRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }
}
