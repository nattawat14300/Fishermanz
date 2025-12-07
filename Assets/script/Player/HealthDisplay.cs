using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    private PlayerHealth playerHealth;

    void Start()
    {
        FindPlayer();
        UpdateHearts();
    }

    void Update()
    {
        if (playerHealth == null) return;
        UpdateHearts();
    }

    void FindPlayer()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("❌ PlayerHealth not found in Scene");
        }
    }

    void UpdateHearts()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null) return;

        int health = playerHealth.health;
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    // ========================
    // สำหรับเรียกตอน Restart
    // ========================
    public void ResetDisplay()
    {
        FindPlayer();
        UpdateHearts();
    }
}
