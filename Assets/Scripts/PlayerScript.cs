using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 6f;
    private float runSpeed = 10f;
    private float baseGravity = 6f;
    private bool isFacingRight = true;
    private Animator animator;

    // Dash
    private bool isDashing = false;
    private bool dashRequested = false;
    private float dashGravity = 0f;
    private float dashSpeedMultiplier = 3f;
    private float dashTime = 0.35f;
    // private float dashCooldown = 5.5f;
    private float dashTimeCounter;

    // private float dashCooldownCounter;
    private float dashDirection;
    [SerializeField] private float dashesleft = 0;
    [SerializeField] private bool isOnGround;
    private Vector2 hitBoxSize = new Vector2(0.93f, 0.1f);
    [SerializeField] private LayerMask floorGrass;
    [SerializeField] private Transform hitBox;
    [SerializeField] public Rigidbody2D rigid2D;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = IsGrounded();
        // Checks what way you are going
        horizontal = Input.GetAxisRaw("Horizontal");

        checkAnimation();

        if (dashesleft > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashRequested = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Flip();
        Dash();

        if (isDashing)
        {
            return;
        }

        // currentSpeed is based on pressing shift or not base speed is faster the holding it
        float currentSpeed = Input.GetKey(KeyCode.S) ? speed : runSpeed;
        // check if u are standing still on the ground to prevent sliding off ledges
        bool idleOnGround = IsGrounded() && horizontal == 0f && rigid2D.linearVelocity.y <= 0.01f;

        if (idleOnGround)
        {
            rigid2D.linearVelocity = Vector2.zero;
            rigid2D.gravityScale = 0f;
        }
        else
        {
            // Normal movement (walk / run)
            rigid2D.gravityScale = baseGravity;
            rigid2D.linearVelocity = new Vector2(horizontal * currentSpeed, rigid2D.linearVelocity.y);
        }
    }

    private void checkAnimation()
    {
        float move = Input.GetAxisRaw("Horizontal");
        bool grounded = IsGrounded();
        float verticalVelY = rigid2D.linearVelocity.y;

        animator.SetBool("isMoving", grounded && move != 0);
        animator.SetBool("isJumpingUp", !grounded && verticalVelY > 0f);
        animator.SetBool("isFalling", !grounded && verticalVelY <= 0f);
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

    private void Dash()
    {
        // Cooldown countdown
        // if (dashCooldownCounter > 0f)
        // {
        //     dashCooldownCounter -= Time.fixedDeltaTime;
        // }

        // Start a dash
        if (dashRequested && dashesleft > 0 && !isDashing && !IsGrounded()) // dashCooldownCounter can be readded.
        {
            isDashing = true;
            dashesleft--;
            dashTimeCounter = dashTime;
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, dashGravity);

            // lock the dash direction
            dashDirection = horizontal != 0f ? horizontal : (isFacingRight ? 1f : -1f);

            rigid2D.gravityScale = dashGravity;
        }
        dashRequested = false;

        // While dashing, the dash overrides all other horizontal movement
        if (isDashing)
        {
            rigid2D.linearVelocity = new Vector2(dashDirection * runSpeed * dashSpeedMultiplier, rigid2D.linearVelocity.y);

            dashTimeCounter -= Time.fixedDeltaTime;
            if (dashTimeCounter <= 0f)
            {
                isDashing = false;
                rigid2D.gravityScale = baseGravity;
            }
        }
    }

    // For debug hitbox
    private void OnDrawGizmos()
    {
        if (hitBox == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitBox.position, hitBoxSize);
    }

    public void AddDash()
    {
        dashesleft++;
    }

    public void SetDash(float dashAmount)
    {
        dashesleft = dashAmount;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(hitBox.position, hitBoxSize, 0f, floorGrass);
    }
}