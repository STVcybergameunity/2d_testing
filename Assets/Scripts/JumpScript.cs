using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private float jumpingPower = 16f;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpbufferTime = 0.2f;
    private float jumpbufferCounter;
    private Vector2 groundCheckSize = new Vector2(0.93f, 0.1f);
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private LayerMask floorGrass;
    // Update is called once per frame
    void Update()
    {
        Jump(); 
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
}
