using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public int health { get { return currentHealth; } }

    [Header("UI")]
    

    [Header("Behavior")]
    public bool disableOnDeath = true;      // ปิด Sprite และ Movement เมื่อ Player ตาย
    public SpriteRenderer playerSprite;     // Assign SpriteRenderer ของ Player
    public MonoBehaviour playerMovement;    // Assign script movement ของ Player
    public Collider2D playerCollider;       // Assign Collider2D ของ Player

    // Internal flags
    private bool isDead = false;
    private bool justHit = false;
    private bool invincible = false;    

    void Start()
    {
        currentHealth = maxHealth;

       

        // Auto-find components ถ้ายังไม่ได้ assign
        if (playerSprite == null) playerSprite = GetComponent<SpriteRenderer>();
        if (playerMovement == null) playerMovement = GetComponent<MonoBehaviour>();
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
    }

  

    private IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.2f); // ป้องกันโดน Damage ซ้ำในเฟรมเดียว
        justHit = false;
    }

    public void TakeDamage(int damage)
    {
        if (invincible || isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(Invincible());
    }


    IEnumerator Invincible()
    {
        invincible = true;
        yield return new WaitForSeconds(1f); // กันโดนซ้ำ 1 วิ
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

        if (playerCollider != null) playerCollider.enabled = false;

        // ✅ แจ้ง CountdownTimer ว่า Player ตาย
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer != null)
        {
            timer.PlayerDied();   // ✅ ให้ CountdownTimer เปิด losePanel และหยุดเวลา
        }
    }


   

    // Optional: Get current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
