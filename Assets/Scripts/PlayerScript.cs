using System;
using NUnit.Framework;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float runSpeed = 12f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpbufferTime = 0.2f;
    private float jumpbufferCounter;

    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask floorGrass;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1f, 0.1f);

    // Update is called once per frame
    void Update()
    {
        // Checks what way you are going
        horizontal = Input.GetAxisRaw("Horizontal");

        Jump();

        Flip();
    }

    private void FixedUpdate()
    {
        rigid2D.linearVelocity = new Vector2(horizontal * speed, rigid2D.linearVelocity.y);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rigid2D.linearVelocity = new Vector2(horizontal * runSpeed, rigid2D.linearVelocity.y);
        }
    }

    private void Jump()
    {
                // Allows the player to jump slightly after faling off a platform
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Allows the player to jump slightly before they hit the floor
        if (Input.GetKey(KeyCode.Space))
        {
            jumpbufferCounter = jumpbufferTime;
        }
        else
        {
            jumpbufferCounter -= Time.deltaTime;
        }

        // If you are on the ground allow jumping
        if (jumpbufferCounter > 0 && coyoteTimeCounter > 0f)
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, jumpingPower);

            jumpbufferCounter = 0f;
        }

        // Allows the player to let go early to start falling down
        if (Input.GetKeyUp(KeyCode.Space) && rigid2D.linearVelocity.y > 0f)
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, rigid2D.linearVelocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.2f, floorGrass);
    }

    private void Flip()
    {
        // Checks where the player is looking and sets the sprite acordingly
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
