using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;   // ความเร็วกระสุน
    public float lifetime = 2f; // เวลากระสุนจะหายไป
    public int damage = 100;    // ดาเมจที่กระสุนทำได้

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float direction = Mathf.Sign(transform.localScale.x); // หาทิศทาง (1 = ขวา, -1 = ซ้าย)
            rb.linearVelocity = new Vector2(speed * direction, 0); // ให้กระสุนเคลื่อนที่ไปในทิศทางที่ตัวละครหันไป
        }
        else
        {
            Debug.LogError("❌ Projectile ไม่มี Rigidbody2D! โปรดตรวจสอบ Prefab.");
        }

        Destroy(gameObject, lifetime); // ลบกระสุนหลังจากเวลาที่กำหนด
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ให้ศัตรูรับดาเมจ
            }
            Destroy(gameObject); // ทำลายกระสุนหลังชน
        }
    }
}
