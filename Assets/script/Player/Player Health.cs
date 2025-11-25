using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int maxHealth = 3;
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
        health -= amount;
        Debug.Log("Player took damage! Current health: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Player died!");
        }
    }

}