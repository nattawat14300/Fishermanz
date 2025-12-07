using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public int health => currentHealth;

    public event Action<int> OnHealthChanged;

    public bool disableOnDeath = true;
    public SpriteRenderer playerSprite;
    public MonoBehaviour playerMovement;
    public Collider2D playerCollider;

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

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();

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

        if (playerCollider != null) playerCollider.enabled = false;

        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer != null) timer.PlayerDied();
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        isDead = false;
        invincible = false;

        if (disableOnDeath)
        {
            if (playerSprite != null) playerSprite.enabled = true;
            if (playerMovement != null) playerMovement.enabled = true;
        }

        if (playerCollider != null) playerCollider.enabled = true;

        OnHealthChanged?.Invoke(currentHealth);
    }
}
