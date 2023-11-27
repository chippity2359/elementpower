using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 5;
    public LayerMask groundLayer; // Layer used to identify the ground
    public Transform groundCheck; // Transform that marks the position to check if grounded
    public float groundCheckRadius = 0.5f; // Radius of the ground check
    public int maxJumps = 3; // Maximum number of jumps

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private float timeSinceLastJump = 0f;
    private const float jumpResetDelay = 0.02f; // 20 milliseconds

    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move the player
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * speed;

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Update the timer
        if (!isGrounded)
        {
            timeSinceLastJump += Time.deltaTime;
        }

        // Reset jump count when grounded
        if (isGrounded && timeSinceLastJump > jumpResetDelay)
        {
            if (jumpCount > 0) {
                Debug.Log("Reset jump count!");
            }
            jumpCount = 0;
            timeSinceLastJump = 0f; // Reset the timer
        }

        // Jump logic
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumps)
        {
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            jumpCount++;
            timeSinceLastJump = 0f; // Reset the timer on jump
            Debug.Log("Jumped! " + jumpCount + " times!");
        }
    }

    void OnDrawGizmos() // Optional: Visualize the ground check in the editor
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
