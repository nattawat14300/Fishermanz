using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("❌ PlayerHealth not found in Scene");
        }
    }

    void Update()
    {
        if (playerHealth == null || hearts.Length == 0) return;

        int health = playerHealth.health;      // property health ใน PlayerHealth
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].gameObject.SetActive(true);

                if (i < health)
                    hearts[i].sprite = fullHeart;
                else
                    hearts[i].sprite = emptyHeart;
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
