using UnityEngine;

public class DashPickupScript : MonoBehaviour
{
    private Vector2 hitBoxSize = new Vector2(1f, 1.5f);

    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private Transform hitBox;

    private bool collected = false;

    void Update()
    {
        if (!collected && IsHit())
        {
            collected = true;

            PlayerScript player = FindFirstObjectByType<PlayerScript>();

            if (player != null)
            {
                player.AddDash();
            }

            Destroy(gameObject);
        }
    }

    private bool IsHit()
    {
        return Physics2D.OverlapBox(
            hitBox.position,
            hitBoxSize,
            0f,
            pickUpLayer
        ) != null;
    }
}