using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }

    void Update()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth NOT ASSIGNED in HealthDisplay");
            return;
        }

        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        if (hearts == null) return;

        // safety: don't iterate past hearts array
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            // ถ้า index น้อยกว่า maxHealth -> แสดงหัวใจ (active)
            hearts[i].gameObject.SetActive(i < maxHealth);

            // ถ้าดัชนีน้อยกว่า health -> full, มิฉะนั้น empty
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
