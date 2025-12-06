using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    // =========================
    //        TIMER
    // =========================
    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float remainingTime = 120f;
    private float startingTime;
    private float elapsedTime;

    public static bool IsGameReady = false;

    // =========================
    //          ORCA
    // =========================
    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;
    public float orcaTime = 60f;
    public float orcaInputDelay = 3f;
    private bool orcaShown = false;
    private bool allowOrcaInput = false;

    // =========================
    //         SENSOR
    // =========================
    [Header("Sensor Input")]
    public ForcePadReader pad;
    public float threshold = 50f;
    private bool sensorLocked = false;

    // =========================
    //         PANELS
    // =========================
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    // =========================
    //         STATE
    // =========================
    private bool timerRunning = true;
    private bool playerAlive = true;
    private bool gameEnded = false;

    // =========================
    //        MANAGERS
    // =========================
    private SpawnerManager spawner;
    private MusicManager music;

    void Start()
    {
        Time.timeScale = 0f;
        IsGameReady = false;

        startingTime = remainingTime;

        spawner = FindObjectOfType<SpawnerManager>();
        music = FindObjectOfType<MusicManager>();

        if (spawner == null)
            Debug.LogError("SpawnerManager not found!");
        if (music == null)
            Debug.LogWarning("MusicManager not found!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (orcaPanel != null) orcaPanel.SetActive(false);

        UpdateTimerUI();
    }

    void Update()
    {
        elapsedTime = startingTime - remainingTime;

        // =========================
        //        ORCA MODE
        // =========================
        if (orcaShown && orcaPanel != null && orcaPanel.activeSelf)
        {
            if (!allowOrcaInput) return;

            bool keyPressed = Input.anyKeyDown || Input.GetMouseButtonDown(0);
            bool sensorPressed = IsAnySensorPressed();

            if (!sensorPressed)
                sensorLocked = false;

            if ((keyPressed || sensorPressed) && !sensorLocked)
            {
                sensorLocked = true;
                OnOrcaNext();
            }

            return;
        }

        // =========================
        //        STOP GAME
        // =========================
        if (!IsGameReady || gameEnded || !timerRunning || !playerAlive)
            return;

        // =========================
        //       TRIGGER ORCA
        // =========================
        if (enableOrca && !orcaShown && elapsedTime >= orcaTime)
        {
            TriggerOrca();
            return;
        }

        // =========================
        //         TIMER
        // =========================
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            OnTimeUp();
        }

        UpdateTimerUI();
    }

    // =========================
    //           ORCA
    // =========================
    void TriggerOrca()
    {
        orcaShown = true;
        timerRunning = false;
        allowOrcaInput = false;
        sensorLocked = true;

        if (orcaPanel != null)
            orcaPanel.SetActive(true);

        Time.timeScale = 0f;

        if (music != null)
            music.FadeOutIntro();

        StartCoroutine(EnableOrcaInput());
    }

    IEnumerator EnableOrcaInput()
    {
        yield return new WaitForSecondsRealtime(orcaInputDelay);
        allowOrcaInput = true;
        sensorLocked = false;
    }

    public void OnOrcaNext()
    {
        if (orcaPanel != null)
            orcaPanel.SetActive(false);

        Time.timeScale = 1f;
        timerRunning = true;
        IsGameReady = true;

        if (music != null)
            music.PlayAfterOrca();

        if (spawner != null)
            spawner.StartSpawning();

        Debug.Log("ORCA NEXT → GAME STARTED");
    }

    // =========================
    //          ENDING
    // =========================
    void OnTimeUp()
    {
        if (gameEnded) return;

        gameEnded = true;
        timerRunning = false;

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void PlayerDied()
    {
        if (gameEnded) return;

        gameEnded = true;
        playerAlive = false;
        timerRunning = false;

        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // =========================
    //            UI
    // =========================
    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int m = Mathf.FloorToInt(remainingTime / 60f);
        int s = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{m:00}:{s:00}";
    }

    // =========================
    //        SENSOR CHECK
    // =========================
    bool IsAnySensorPressed()
    {
        if (pad == null) return false;

        return pad.f1 > threshold ||
               pad.f2 > threshold ||
               pad.f3 > threshold ||
               pad.f4 > threshold ||
               pad.f5 > threshold;
    }
}
