using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask floorGrass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checks what way you are going
        horizontal = Input.GetAxisRaw("Horizontal");

        // If you are on the ground allow jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, jumpingPower);
        }

        // Allows the player to let go early to start falling down
        if (Input.GetButtonUp("Jump") && rigid2D.linearVelocity.y > 0f)
        {
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, rigid2D.linearVelocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rigid2D.linearVelocity = new Vector2(horizontal * speed, rigid2D.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, floorGrass);
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
