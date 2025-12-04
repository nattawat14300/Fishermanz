using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CountdownTimer : MonoBehaviour
{
    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;     // เปิด/ปิด ระบบ Orca
    public float orcaTime = 60f;       // เวลาที่ Orca จะโผล่ (1 นาที)
    private bool orcaShown = false;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;
    public static bool IsGameReady = false;
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool timerRunning = true;
    private bool playerAlive = true;

    private void Start()
    {
        IsGameReady = false;

        if (timerText == null)
            Debug.LogError("Timer Text not assigned!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateTimerUI();
    }

    private void TriggerOrca()
    {
        orcaShown = true;
        timerRunning = false;

        if (orcaPanel != null)
            orcaPanel.SetActive(true);

        Time.timeScale = 0f; // หยุดเกม
    }


    private void Update()
    {
        if (!timerRunning || !playerAlive) return;

        if (enableOrca && !orcaShown && remainingTime <= orcaTime)
        {
            TriggerOrca();
        }

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

    public void OnOrcaNext()
    {
        if (orcaPanel != null)
            orcaPanel.SetActive(false);

        Time.timeScale = 1f;   // เล่นเกมต่อ
        timerRunning = true;

        IsGameReady = true;
    }

}
