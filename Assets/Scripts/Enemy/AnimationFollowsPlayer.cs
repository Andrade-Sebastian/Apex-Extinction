using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFollowsPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Transform player;  

    void Start()
    {
        spriteRenderer = GetComponent < SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null)
            return;  // Player not found, exit the Update function.

        // Move towards the player
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

        // Flip the sprite based on the direction
        if (direction.x > 0)
        {
            // Player is to the right, so face right
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            // Player is to the left, so face left
            spriteRenderer.flipX = true;
        }
        // Note: If direction.x == 0, the zombie will keep its current facing direction.

        // Rest of the code remains the same...
    }
}

