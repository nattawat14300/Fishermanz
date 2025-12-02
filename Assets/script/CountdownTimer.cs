using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool timerRunning = true;
    private bool playerAlive = true;

    private void Start()
    {
        if (timerText == null)
            Debug.LogError("Timer Text not assigned!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateTimerUI();
    }

    private void Update()
    {
        if (!timerRunning || !playerAlive) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                OnTimeUp();
            }
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnTimeUp()
    {
        timerRunning = false;
        if (playerAlive && winPanel != null)
        {
            winPanel.SetActive(true); // ชนะเพราะอยู่ครบเวลา
        }
    }

    // เรียกเมื่อผู้เล่นตายก่อนหมดเวลา
    public void PlayerDied()
    {
        playerAlive = false;
        timerRunning = false;

        if (remainingTime > 0 && losePanel != null)
        {
            losePanel.SetActive(true); // แพ้เพราะตายก่อนเวลา
        }
    }
}
