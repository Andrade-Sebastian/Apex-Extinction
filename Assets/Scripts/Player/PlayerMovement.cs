using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [HideInInspector]
    public float lastHorizontalVector;

    [HideInInspector]
    public float lastVerticalVector;

    [HideInInspector]
    public Vector2 moveDir;

    [HideInInspector]
    public Vector2 lastMovedDirVector;

    // references
    Rigidbody2D rb;
    PlayerStats Ps; 
    // Start is called before the first frame update
    void Start()
    {
        Ps = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMovedDirVector = new Vector2(1, 0f); // default
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();

    }

    private void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedDirVector = new Vector2(lastHorizontalVector, 0f); //last moved in x dir
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedDirVector = new Vector2(0f, lastVerticalVector);
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedDirVector = new Vector2(lastHorizontalVector, lastVerticalVector); //while character is moving
        }
    }

    void Move()
    {

        rb.velocity = new Vector2(moveDir.x * Ps.currentMoveSpeed, moveDir.y * Ps.currentMoveSpeed);

    }

}
