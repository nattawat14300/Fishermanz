using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameManager gameManager;
    public int health;
    public int maxHealth = 3;
    public GameObject gameOverUI;
    public GameObject player;

    public SpriteRenderer playerSr;
    public PlayerMovementSmooth playerMovement;

    private bool isDead = false;

    void Start()
    {
        health = maxHealth;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        Debug.Log("Player took damage! Current health: " + health);

        if (health <= 0 && !isDead)
        {
            isDead = true;
            if (gameManager != null) gameManager.gameOver();
            if (playerSr != null) playerSr.enabled = false;
            if (playerMovement != null) playerMovement.enabled = false;
            gameObject.SetActive(false);
        }
    }

   

}