using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    // =========================
    //           ORCA
    // =========================
    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;
    public float orcaTime = 60f;
    public float orcaInputDelay = 2f;

    public static bool IsGameReady = false;

    private bool orcaShown = false;
    private bool orcaUsed = false;
    private bool allowOrcaInput = false;
    private bool waitForSensorRelease = false;
    private bool sensorLocked = false;

    // =========================
    //           TIMER
    // =========================
    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float remainingTime = 120f;

    private float startingTime;
    private float elapsedTime;
    private bool timerRunning = true;

    // =========================
    //          SENSOR
    // =========================
    [Header("Sensor Input")]
    public ForcePadReader pad;
    public float threshold = 50f;

    // =========================
    //          PANELS
    // =========================
    public GameObject winPanel;
    public GameObject losePanel;

    // =========================
    //          STATE
    // =========================
    private bool gameEnded = false;
    private bool playerAlive = true;

    // =========================
    //         MANAGERS
    // =========================
    private SpawnerManager spawner;
    private MusicManager music;

    // =========================
    //           START
    // =========================
    void Start()
    {
        Time.timeScale = 1f;
        IsGameReady = false;

        startingTime = remainingTime;

        spawner = FindObjectOfType<SpawnerManager>();
        music = FindObjectOfType<MusicManager>();

        winPanel?.SetActive(false);
        losePanel?.SetActive(false);
        orcaPanel?.SetActive(false);

        UpdateTimerUI();
    }

    // =========================
    //           UPDATE
    // =========================
    void Update()
    {
        if (gameEnded || !playerAlive)
            return;

        // ===== ORCA MODE =====
        if (orcaShown)
        {
            HandleOrcaInput();
            return;
        }

        // ===== TIMER =====
        if (timerRunning)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0f);
            UpdateTimerUI();
        }

        elapsedTime = startingTime - remainingTime;

        // ===== TRIGGER ORCA (ครั้งเดียว) =====
        if (enableOrca && !orcaUsed && elapsedTime >= orcaTime)
        {
            TriggerOrca();
            return;
        }

        // ===== TIME UP =====
        if (remainingTime <= 0f)
        {
            OnTimeUp();
        }
    }

    // =========================
    //           ORCA
    // =========================
    void TriggerOrca()
    {
        orcaUsed = true;
        orcaShown = true;

        timerRunning = false;
        allowOrcaInput = false;
        waitForSensorRelease = true;
        sensorLocked = true;

        orcaPanel.SetActive(true);
        Time.timeScale = 0f;

        music?.FadeOutIntro();
        StartCoroutine(EnableOrcaInputAfterDelay());

        Debug.Log("ORCA PANEL SHOW");
    }

    IEnumerator EnableOrcaInputAfterDelay()
    {
        yield return new WaitForSecondsRealtime(orcaInputDelay);
        allowOrcaInput = true;
        sensorLocked = false;
        Debug.Log("ORCA INPUT ENABLED");
    }

    void HandleOrcaInput()
    {
        bool sensorPressed = IsAnySensorPressed();
        bool otherPressed = Input.anyKeyDown || Input.GetMouseButtonDown(0);

        // ต้องปล่อย sensor ก่อน 1 ครั้ง
        if (waitForSensorRelease)
        {
            if (!sensorPressed)
                waitForSensorRelease = false;

            return;
        }

        if (!allowOrcaInput)
            return;

        if ((sensorPressed || otherPressed) && !sensorLocked)
        {
            sensorLocked = true;
            OnOrcaNext();
        }

        if (!sensorPressed)
            sensorLocked = false;
    }

    void OnOrcaNext()
    {
        orcaPanel.SetActive(false);
        orcaShown = false;

        Time.timeScale = 1f;
        timerRunning = true;
        IsGameReady = true;

        music?.PlayAfterOrca();
        spawner?.StartSpawning();

        Debug.Log("ORCA NEXT → GAME RESUME");
    }

    // =========================
    //           ENDING
    // =========================
    void OnTimeUp()
    {
        if (gameEnded) return;

        gameEnded = true;
        timerRunning = false;

        winPanel?.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("TIME UP → WIN");
    }

    public void PlayerDied()
    {
        if (gameEnded) return;

        gameEnded = true;
        playerAlive = false;
        timerRunning = false;

        losePanel?.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("PLAYER DIED → LOSE");
    }

    // =========================
    //            UI
    // =========================
    void UpdateTimerUI()
    {
        if (!timerText) return;

        int m = Mathf.FloorToInt(remainingTime / 60f);
        int s = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{m:00}:{s:00}";
    }

    // =========================
    //        SENSOR CHECK
    // =========================
    bool IsAnySensorPressed()
    {
        if (!pad) return false;

        return pad.f1 > threshold ||
               pad.f2 > threshold ||
               pad.f3 > threshold ||
               pad.f4 > threshold ||
               pad.f5 > threshold;
    }
}
