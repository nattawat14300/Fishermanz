using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public CountdownTimer timer;

    [Header("Player Components")]
    public SpriteRenderer PlayerSr;
    public PlayerStepMove playerStepMove;

    [Header("UI")]
    public GameObject gameOverPanel; // << assign panel ใน Inspector

    void Start()
    {
        health = maxHealth;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;

            if (PlayerSr != null) PlayerSr.enabled = false;
            if (playerStepMove != null) playerStepMove.enabled = false;

            if (gameOverPanel != null) gameOverPanel.SetActive(true);

            if (timer != null)
                timer.PlayerDied(); // แจ้ง Timer ว่าแพ้
        }
    }
}
