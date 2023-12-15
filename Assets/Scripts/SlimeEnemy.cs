using UnityEngine;
using System.Collections;


public class SlimeEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 5f;
    public Transform groundDetection;
    public Animator animator; // Animator for the slime
    private Rigidbody2D rb;

    private bool movingRight = true;
    private float initialX;

    void Start()
    {
        initialX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        if (!groundInfo.collider || Mathf.Abs(initialX - transform.position.x) >= patrolDistance)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

            initialX = transform.position.x; // Reset patrol distance
        }
    }

    // Example of an attack method
    public void Hit()
    {
        animator.SetTrigger("HitTrigger");
        rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * 4, ForceMode2D.Impulse);
        moveSpeed = 5f;
        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(3);
        moveSpeed = 2f;
    }

    // Example of an attack method
    void AttackPlayer()
    {
        // Attack logic here
    }

    // OnTriggerEnter2D or OnCollisionEnter2D can be used to detect player and attack
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }
}
