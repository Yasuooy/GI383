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
    [SerializeField] private float attackCooldown = 1.5f; // คูลดาวน์ระหว่างโจมตี
    private float nextAttackTime = 0f;

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

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attackCooldown;
            Invoke("ResetAttackTrigger", 0.1f); // ล้างค่า Trigger หลังโจมตี
        }
    }

    void ResetAttackTrigger()
    {
        animator.ResetTrigger("Attack1");
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
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false; // ปิดการชนกัน
        this.enabled = false; // ปิดการทำงานของสคริปต์
        Destroy(gameObject, 1.5f); // รอ 1.5 วิ ก่อนลบออก
    }



    private void FlipSprite(float direction)
    {
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
