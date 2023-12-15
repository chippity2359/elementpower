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
    public Animator animator; // Animator for the player
    public Transform attackPoint; // Point from where the attack will be detected
    public float attackRange = 0.5f; // Range of the attack
    public LayerMask enemyLayers; // Layer used to identify enemies


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
        
        if (Input.GetKeyDown(KeyCode.Space)) // Using space bar for attack
        {
            Attack();
        }
  
    }
    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length > 0) {
            Debug.Log("We hit " + hitEnemies.Length + " enemies!");
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            // Add logic here to damage enemy

            enemy.GetComponent<SlimeEnemy>().Hit();
        }
    }

    void OnDrawGizmos() // Optional: Visualize the ground check in the editor
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}   