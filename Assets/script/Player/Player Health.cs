using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public int health { get { return currentHealth; } }

    [Header("Behavior")]
    public bool disableOnDeath = true;
    public SpriteRenderer playerSprite;
    public MonoBehaviour playerMovement;
    public Collider2D playerCollider;

    // Internal flags
    private bool isDead = false;
    private bool invincible = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (playerSprite == null) playerSprite = GetComponent<SpriteRenderer>();
        if (playerMovement == null) playerMovement = GetComponent<MonoBehaviour>();
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (invincible || isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
            Die();

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (disableOnDeath)
        {
            if (playerSprite != null) playerSprite.enabled = false;
            if (playerMovement != null) playerMovement.enabled = false;
        }

        if (playerCollider != null)
            playerCollider.enabled = false;

        // แจ้ง CountdownTimer ว่า Player ตาย
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer != null)
            timer.PlayerDied();
    }

    // ===========================
    // ฟังก์ชันสำหรับ Restart
    // ===========================
    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        isDead = false;

        // รีเซ็ต Sprite และ Movement
        if (disableOnDeath)
        {
            if (playerSprite != null) playerSprite.enabled = true;
            if (playerMovement != null) playerMovement.enabled = true;
        }

        // รีเซ็ต Collider
        if (playerCollider != null) playerCollider.enabled = true;

        // รีเซ็ต Invincible
        invincible = false;
    }
}
