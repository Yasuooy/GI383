using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;   // ความเร็วกระสุน
    public float lifetime = 2f; // ระยะเวลาที่กระสุนจะหายไป
    public int damage = 100;    // ดาเมจที่ทำได้

    private Rigidbody2D rb; // ตัวแปร Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed; // ทำให้กระสุนเคลื่อนที่ไปข้างหน้า
        }
        else
        {
            Debug.LogError("Projectile ไม่มี Rigidbody2D! โปรดตรวจสอบ Prefab.");
        }

        Destroy(gameObject, lifetime); // ทำลายกระสุนหลังจากเวลาที่กำหนด
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
