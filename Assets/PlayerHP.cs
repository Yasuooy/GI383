using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private Vector3 safeZonePosition;  // ตำแหน่ง Safe Zone ล่าสุด
    private bool hasEnteredSafeZone = false;  // เช็คว่าเคยเข้า Safe Zone หรือยัง

    void Start()
    {
        currentHealth = maxHealth;
        safeZonePosition = transform.position;  // เริ่มต้นให้เซฟโซนอยู่ที่ตำแหน่งแรกของผู้เล่น
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Q))
            TakeDamage(10);

        if (Input.GetKeyDown(KeyCode.E))
            Die();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Player takes damage: " + damage + ". Current Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public void InstantDeath()
    {
        currentHealth = 0;
        Die();
    }



    private void Die()
    {
        if (hasEnteredSafeZone)
        {
            RespawnAtSafeZone();  // ถ้าเคยเข้า Safe Zone แล้วจะกลับไปจุดเซฟ
        }
        else
        {
            isDead = true;
            Debug.Log("Player has died. Game Over.");
            Time.timeScale = 0;  // หยุดเกม
        }
    }

    private void RespawnAtSafeZone()
    {
        Debug.Log("Player died, respawning at Safe Zone...");
        currentHealth = maxHealth;
        transform.position = safeZonePosition;  // กลับไปยังตำแหน่งเซฟ
        isDead = false;
    }

    public void SetSafeZone(Vector3 position)
    {
        safeZonePosition = position;
        hasEnteredSafeZone = true;
        Debug.Log("Safe Zone updated to: " + position);
    }
}

