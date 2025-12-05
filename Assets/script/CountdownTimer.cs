using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    
    [SerializeField] float elapsedTime = 0f;
    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;
    public float orcaTime = 55f;
    private bool orcaShown = false;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 120f;

    private float startingTime;

    public static bool IsGameReady = false;


    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private SpawnerManager spawnerManager;
    private bool initialSpawnStarted = false;

    // ✅ ตัวแปรควบคุมสถานะ (ที่คุณลบหายไป)
    private bool timerRunning = true;
    private bool playerAlive = true;
    private bool gameEnded = false;

    // 🎵 Music Manager
    private MusicManager music;

    private void Start()
    {
        IsGameReady = false;
        Time.timeScale = 0f;

        startingTime = remainingTime;

        spawnerManager = FindObjectOfType<SpawnerManager>(); 
        
        if (spawnerManager == null) Debug.LogError("SpawnerManager not found!");
        music = FindObjectOfType<MusicManager>();

        if (timerText == null)
            Debug.LogError("Timer Text not assigned!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        

        UpdateTimerUI();
    }

    private void Update()
    {

        elapsedTime = startingTime - remainingTime;
        if (gameEnded || !timerRunning || !playerAlive || !IsGameReady) return;

        // ✅ ถ้า Orca Panel แสดงอยู่ → กดปุ่มอะไรก็ได้ = Next
        if (orcaShown && orcaPanel != null && orcaPanel.activeSelf)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                OnOrcaNext();
            }
            return; // หยุดไม่ให้ Timer เดินต่อ
        }

        if (gameEnded || !timerRunning || !playerAlive) return;

        float oneMinuteElapsed = remainingTime - 60f; 

        if (!initialSpawnStarted && elapsedTime >= 60f && spawnerManager != null)
        {
            spawnerManager.ChangeSpawnRate(3.0f, 5.0f); 
            initialSpawnStarted = true;
            Debug.Log("CountdownTimer: Initial Spawning has started after 1 minute elapsed.");
        }
        // เงื่อนไข Orca:
        if (enableOrca && !orcaShown && elapsedTime >= orcaTime)
        {
            if (spawnerManager != null) spawnerManager.StopSpawning();
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
