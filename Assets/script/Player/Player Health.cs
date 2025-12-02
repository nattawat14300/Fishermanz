using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    public int maxHealth = 3;
     public int health;          // optional attribute ReadOnly requires custom inspector; left for clarity

    [Header("References")]
    public CountdownTimer timer;           // assign your CountdownTimer (optional)
    public SpriteRenderer playerSr;        // assign SpriteRenderer (optional)
    public PlayerStepMove playerStepMove;  // assign movement script (optional)
    public Collider2D playerCollider;      // assign player's collider (optional)

    [Header("UI")]
    public GameObject gameOverPanel;       // assign the Game Over / Lose panel

    [Header("Behavior")]
    public bool disableOnDeath = true;     // disable sprite & movement when die
    public bool disableColliderOnDeath = true;

    // internal guard to avoid double-death
    bool isDead = false;

    void Start()
    {
        health = maxHealth;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // auto-find commonly-missed components if not assigned
        if (playerSr == null) playerSr = GetComponent<SpriteRenderer>();
        if (playerStepMove == null) playerStepMove = GetComponent<PlayerStepMove>();
        if (playerCollider == null) playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        health = Mathf.Max(0, health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // disable visuals / movement if desired
        if (disableOnDeath)
        {
            if (playerSr != null) playerSr.enabled = false;
            if (playerStepMove != null) playerStepMove.enabled = false;
        }

        if (disableColliderOnDeath && playerCollider != null)
            playerCollider.enabled = false;

        // show panel (game over)
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // notify timer that player died (so it can show lose panel if remainingTime > 0)
        if (timer != null)
            timer.PlayerDied();

        // OPTIONAL: you may want to pause other game systems (enemies) here.
        // Example: Stop all EnemyMovement scripts:
        // var enemies = FindObjectsOfType<EnemyMovement>();
        // foreach (var e in enemies) e.StopMove(true);
    }

    // Optional helper to revive / restart the level
    public void RestartLevel()
    {
        // hide panel
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // restore time scale in case other code changed it
        Time.timeScale = 1f;

        // reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
