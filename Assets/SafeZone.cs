using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetSafeZone(transform.position);  // อัปเดต Safe Zone เป็นตำแหน่งใหม่
                Debug.Log("✅ Safe Zone Updated: " + transform.position);
            }
        }
    }
}
