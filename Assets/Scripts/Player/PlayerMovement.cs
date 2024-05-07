using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class PlayerMovement : MonoBehaviour
{
    //movement
    public float moveSpeed;

    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir; //used to check direction player is moving
    [HideInInspector]
    public Vector2 lastMovedVector;

    //references
    Rigidbody2D rb;
    PlayerStats player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // if this is not set and the player does not move, the projectile will have no momentum
    }

    void Update()
    {
        InputManagement();
    }

    //better for physics updates- not based on framerate
    void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        // disable movement inputs if game over is true
        if (GameManager.instance.isGamerOver)
        {
            return;
        }

        float moveX, moveY;
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (VirtualJoystick.CountActiveInstances() > 0)
        {
            moveX += VirtualJoystick.GetAxisRaw("Horizontal");
            moveY += VirtualJoystick.GetAxisRaw("Vertical");
        }

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f); // last movement x
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector); // last movement y
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector); // while moving
        }
    }

    void Move()
    {
        // disable movement inputs if game over is true
        if (GameManager.instance.isGamerOver)
        {
            return;
        }

        //apply velocity to rigidbody
        rb.velocity = new Vector2(moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed);
    }
}