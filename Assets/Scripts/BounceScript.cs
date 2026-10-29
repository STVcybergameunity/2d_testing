using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [SerializeField] private Transform hitBox;
    [SerializeField] private LayerMask Bounce;
    private PlayerScript player;
    private float bounceVelocity = 50;
    private Vector2 hitBoxSize = new Vector2(1f, 1.5f);
    void Start()
    {
        player = FindFirstObjectByType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHit())
        {
            Vector2 velocity = player.rigid2D.linearVelocity;
            velocity.y = bounceVelocity;
            player.rigid2D.linearVelocity = velocity;
        }
    }

    private bool IsHit()
    {
        return Physics2D.OverlapBox(
            hitBox.position,
            hitBoxSize,
            0f,
            Bounce
        ) != null;
    }
}
