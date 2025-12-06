using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // =========================
    //        HP SETTINGS
    // =========================
    [Header("HP Settings")]
    public int maxHealth = 3;
    private int currentHealth;
    public int health { get { return currentHealth; } }

    // =========================
    //        BEHAVIOR
    // =========================
    [Header("Behavior")]
    public bool disableOnDeath = true;      // ปิด Sprite + Movement เมื่อ Player ตาย
    public SpriteRenderer playerSprite;
    public MonoBehaviour playerMovement;
    public Collider2D playerCollider;

    // =========================
    //        FLAGS
    // =========================
    private bool isDead = false;
    private bool invincible = false;

    // =========================
    //        START
    // =========================
    void Start()
    {
        currentHealth = maxHealth;

        // Auto Get ถ้ายังไม่ Assign
        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        if (playerCollider == null)
            playerCollider = GetComponent<Collider2D>();

        // ⚠ ไม่ auto-get playerMovement แบบสุ่ม ให้ Assign เองใน Inspector จะปลอดภัยกว่า
    }

    // =========================
    //      TAKE DAMAGE
    // =========================
    public void TakeDamage(int damage)
    {
        if (isDead || invincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(Invincible());
    }

    // =========================
    //      INVINCIBLE
    // =========================
    IEnumerator Invincible()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

    // =========================
    //          DIE
    // =========================
    void Die()
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

        // แจ้ง CountdownTimer ให้เปิด losePanel
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer != null)
            timer.PlayerDied();
    }

    // =========================
    //      GET CURRENT HP
    // =========================
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
