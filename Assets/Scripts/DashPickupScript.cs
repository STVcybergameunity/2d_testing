using UnityEngine;

public class DashPickupScript : MonoBehaviour
{
    private Vector2 hitBoxSize = new Vector2(1f, 1.5f);

    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private Transform hitBox;

    private bool collected = false;
    private PlayerScript player;
    private SpriteRenderer[] sprites;
    private Vector3 MousePos;

    void Start()
    {
        player = FindFirstObjectByType<PlayerScript>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // if (Input.GetMouseButtonDown(0))
        // {
        //     MousePos = Input.mousePosition;

        //     player.rigid2D.position.
        // }

        if (!collected && IsHit())
        {
            collected = true;
            player.AddDash();
            SetVisible(false);
        }

        if (collected && player.rigid2D.position.y < -6)
        {
            player.SetDash(0);
            collected = false;
            SetVisible(true);
        }
    }

    private void SetVisible(bool visible)
    {
        foreach (var sprite in sprites)
            sprite.enabled = visible;
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