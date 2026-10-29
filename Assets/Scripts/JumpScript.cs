using System;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private float jumpingPower = 21f;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpbufferTime = 0.2f;
    private float jumpbufferCounter;
    private float doubleJumpCount;
    private Vector2 groundCheckSize = new Vector2(0.93f, 0.1f);
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private LayerMask floorGrass;
    [SerializeField] private bool isInAir = false;
    // Update is called once per frame
    void Update()
    {
        Jump(); 
    }

    private void Jump()
    {
        bool grounded = IsGrounded();
        // Allows the player to jump slightly after faling off a platform
        isInAir = !grounded;
        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
            doubleJumpCount = 1;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Allows the player to jump slightly before they hit the floor
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jumpbufferCounter = jumpbufferTime;
        }
        else
        {
            jumpbufferCounter -= Time.deltaTime;
        }

        // If you are on the ground allow jumping
        if (jumpbufferCounter > 0 && coyoteTimeCounter > 0f && (grounded || rigid2D.linearVelocity.y <= 0))
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, jumpingPower);

            jumpbufferCounter = 0f;
        }

        if (isInAir && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && coyoteTimeCounter < 0f && doubleJumpCount > 0)
        {
            rigid2D.linearVelocityY = 0;
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, jumpingPower);
            doubleJumpCount --;
        }

        // Allows the player to let go early to start falling down
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) && rigid2D.linearVelocity.y > 0f)
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, rigid2D.linearVelocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, floorGrass);
    }
}