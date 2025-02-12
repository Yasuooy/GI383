using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private Vector3 safeZonePosition;
    private bool hasSafeZone = false;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab; // Prefab กระสุน
    [SerializeField] private Transform firePoint; // จุดที่กระสุนออกไป
    [SerializeField] private float fireRate = 0.5f; // คูลดาวน์การยิง
    [SerializeField] private float bulletSpeed = 10f; // ความเร็วกระสุน
    private float nextFireTime = 0f;
    private int facingDirection = 1; // หันซ้าย/ขวา

    [Header("Movement Settings")]
    [SerializeField] private float speed = 4.0f;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        safeZonePosition = transform.position; // เซฟตำแหน่งเกิดเริ่มต้น
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX != 0)
        {
            facingDirection = (int)Mathf.Sign(inputX); // -1 หรือ 1
            transform.localScale = new Vector3(facingDirection, 1, 1); // หันซ้าย/ขวา
        }

        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

        // อัปเดตแอนิเมชัน
        animator.SetInteger("AnimState", inputX == 0 ? 0 : 1);
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab && firePoint)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                rbBullet.linearVelocity = new Vector2(facingDirection * bulletSpeed, 0); // กำหนดทิศทางของกระสุน
            }
            else
            {
                Debug.LogError("❌ Projectile ไม่มี Rigidbody2D! โปรดตรวจสอบ Prefab.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"⚠️ Player takes damage: {damage}. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (hasSafeZone)
        {
            RespawnAtSafeZone();
        }
        else
        {
            isDead = true;
            Debug.Log("💀 Player has died. Game Over.");
            Time.timeScale = 0;
        }
    }

    private void RespawnAtSafeZone()
    {
        Debug.Log("🟢 Respawning at Safe Zone...");
        currentHealth = maxHealth;
        transform.position = safeZonePosition;
        isDead = false;
    }

    public void SetSafeZone(Vector3 position)
    {
        safeZonePosition = position;
        hasSafeZone = true;
        Debug.Log("✅ Safe Zone Updated: " + position);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
