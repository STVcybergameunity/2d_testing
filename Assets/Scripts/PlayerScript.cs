using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float runSpeed = 10f;
    private float baseGravity = 6f;
    private bool isFacingRight = true;

    // Dash
    private bool isDashing = false;
    private bool dashRequested = false;
    private float dashGravity = 0f;
    private float dashSpeedMultiplier = 3f;
    private float dashTime = 0.35f;
    private float dashCooldown = 5.5f;
    private float dashTimeCounter;
    private float dashCooldownCounter;
    private float dashDirection;
    [SerializeField] private float dashesleft = 0;

    private Vector2 hitBoxSize = new Vector2(0.93f, 0.1f);
    [SerializeField] private LayerMask floorGrass;
    [SerializeField] private Transform hitBox;
    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private LayerMask pickUpLayer;

    // Update is called once per frame
    void Update()
    {
        // Checks what way you are going
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            dashRequested = true;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (dashesleft > 0)
        {
            Dash();
        }

        if (isDashing)
        {
            return;
        }
        else
        {
            // Normal movement (walk / run)
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;
            rigid2D.linearVelocity = new Vector2(horizontal * currentSpeed, rigid2D.linearVelocity.y);
        }
    }

    private void Flip()
    {
        // Don't turn around mid-dash, the direction is locked
        if (isDashing) return;

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
        if (dashCooldownCounter > 0f)
        {
            dashCooldownCounter -= Time.fixedDeltaTime;
        }

        // Start a dash
        if (dashRequested && !isDashing && dashCooldownCounter <= 0f && !IsGrounded())
        {
            isDashing = true;
            dashTimeCounter = dashTime;
            rigid2D.linearVelocity = new Vector2(rigid2D.linearVelocity.x, dashGravity);

            // Lock direction: input direction, or the way the player is facing if standing still
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
                dashCooldownCounter = dashCooldown; // cooldown starts when the dash ends
            }
        }
    }

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

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(hitBox.position, hitBoxSize, 0.2f, floorGrass);
    }
}