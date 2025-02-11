using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float detectionRange = 5f;
    private int currentHealth;
    private Animator animator;
    private Transform player;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("Run", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        FlipSprite(direction.x);
    }

    void AttackPlayer()
    {
        animator.SetBool("Run", false);
        animator.SetTrigger("Attack1");
    }

    void Idle()
    {
        animator.SetBool("Run", false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("TakeHit");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;  // ปิดการทำงานของ EnemyController
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
