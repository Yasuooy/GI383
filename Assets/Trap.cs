using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log("💀 Player hit Death Zone!");

                // ทำให้ผู้เล่นตาย (และ Respawn ถ้ามี Safe Zone)
                player.TakeDamage(player.GetMaxHealth());
            }
        }
    }
}
