using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 movement; // Store movement input

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Capture movement input
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Move the character
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}