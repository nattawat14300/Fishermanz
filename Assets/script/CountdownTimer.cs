using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;
    public float orcaTime = 60f;
    private bool orcaShown = false;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;

    public static bool IsGameReady = false;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    // ✅ ตัวแปรควบคุมสถานะ (ที่คุณลบหายไป)
    private bool timerRunning = true;
    private bool playerAlive = true;
    private bool gameEnded = false;
    
    // 🎵 Music Manager
    private MusicManager music;

    private void Start()
    {
        Time.timeScale = 1f;
        IsGameReady = false;

        music = FindObjectOfType<MusicManager>();

        if (timerText == null)
            Debug.LogError("Timer Text not assigned!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateTimerUI();
    }

    private void Update()
    {
        if (gameEnded || !timerRunning || !playerAlive) return;

        if (enableOrca && !orcaShown && remainingTime <= orcaTime)
        {
            TriggerOrca();
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            OnTimeUp();
        }

        UpdateTimerUI();
    }

    private void TriggerOrca()
    {
        orcaShown = true;
        timerRunning = false;

        if (orcaPanel != null)
            orcaPanel.SetActive(true);

        if (music != null)
            music.FadeOutIntro();

        Time.timeScale = 0f; // หยุดเกม
    }

    private void OnTimeUp()
    {
        if (gameEnded) return;

        timerRunning = false;
        gameEnded = true;

        if (music != null)
            music.StopMusic();


        if (playerAlive && winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;   // ชนะ -> หยุดเกม
        }
    }

    // 🔴 เรียกจาก PlayerHealth
    public void PlayerDied()
    {
        if (gameEnded) return;

        playerAlive = false;
        timerRunning = false;
        gameEnded = true;

        if (music != null)
            music.StopMusic();


        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;  // แพ้ -> หยุดเกม
        }
    }

    public void OnOrcaNext()
    {
        if (orcaPanel != null)
            orcaPanel.SetActive(false);

        Time.timeScale = 1f;
        timerRunning = true;
        IsGameReady = true;

        if (music != null)
            music.PlayAfterOrca();   // ✅ เปลี่ยนเพลงหลัง Orca
    }



    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
