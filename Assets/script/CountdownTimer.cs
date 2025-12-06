using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] float elapsedTime = 0f;

    [Header("Orca Panel")]
    public GameObject orcaPanel;
    public bool enableOrca = true;
    public float orcaTime = 55f;
    private bool orcaShown = false;

    [Header("Orca Input Delay")]
    public float orcaInputDelay = 3f;   // ✅ หน่วง 3 วินาทีก่อนกด Next
    private bool allowOrcaInput = false;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 120f;
    private float startingTime;

    public static bool IsGameReady = false;

    [Header("Sensor Input (Orca Next)")]
    public ForcePadReader pad;
    public float threshold = 50f;   // ✅ ปรับตามแรงกด
    private bool sensorLocked = false;
    
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool timerRunning = true;
    private bool playerAlive = true;
    private bool gameEnded = false;

    private SpawnerManager spawner;
    private MusicManager music;

    void Start()
    {
        // ===== INITIALIZE =====
        IsGameReady = false;
        Time.timeScale = 0f;

        startingTime = remainingTime;

        music = FindObjectOfType<MusicManager>();
        spawner = FindObjectOfType<SpawnerManager>();

        if (spawner == null)
            Debug.LogError("SpawnerManager not found in scene!");

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateTimerUI();
    }

    void Update()
    {
        elapsedTime = startingTime - remainingTime;

        // =============================
        //        ORCA PANEL MODE
        // =============================
        if (orcaShown && orcaPanel != null && orcaPanel.activeSelf)
        {
            // ✅ ยังไม่ครบ 3 วิ → ห้ามกด
            if (!allowOrcaInput)
                return;

            bool keyPressed = Input.anyKeyDown || Input.GetMouseButtonDown(0);
            bool sensorPressed = IsAnySensorPressed();

            if (pad != null)
                Debug.Log($"SENSOR = {pad.f1}, {pad.f2}, {pad.f3}, {pad.f4}, {pad.f5}");

            // ✅ ปล่อย sensor ก่อนเพื่อกันเด้ง
            if (!sensorPressed)
                sensorLocked = false;

            // ✅ ครบเวลาแล้ว + ยังไม่ lock → Next
            if ((keyPressed || sensorPressed) && !sensorLocked)
            {
                sensorLocked = true;
                OnOrcaNext();
            }

            return;
        }

        // =============================
        //        WAIT GAME READY
        // =============================
        if (!IsGameReady || gameEnded || !timerRunning || !playerAlive)
            return;

        // =============================
        //         TRIGGER ORCA
        // =============================
        if (enableOrca && !orcaShown && elapsedTime >= orcaTime)
        {
            TriggerOrca();
        }

        // =============================
        //           TIMER
        // =============================
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            OnTimeUp();
        }

        UpdateTimerUI();
    }

    // ======================
    //        ORCA
    // ======================
    private void TriggerOrca()
    {
        orcaShown = true;
        timerRunning = false;
        allowOrcaInput = false;   // ✅ ล็อก input ตอน panel เปิด

        if (orcaPanel != null)
            orcaPanel.SetActive(true);

        Time.timeScale = 0f;

        if (music != null)
            music.FadeOutIntro();

        StartCoroutine(EnableOrcaInputAfterDelay());

        Debug.Log("ORCA PANEL SHOWING");
    }

    IEnumerator EnableOrcaInputAfterDelay()
    {
        yield return new WaitForSecondsRealtime(orcaInputDelay); // ✅ ใช้ realtime เพราะ TimeScale = 0
        allowOrcaInput = true;
        Debug.Log("ORCA INPUT ENABLED");
    }

    public void OnOrcaNext()
    {
        Debug.Log("ORCA NEXT");

        if (orcaPanel != null)
            orcaPanel.SetActive(false);

        Time.timeScale = 1f;
        timerRunning = true;
        IsGameReady = true;

        if (music != null)
            music.PlayAfterOrca();

        // ✅ เริ่ม Spawn ด้วยค่าจาก Inspector
        if (spawner != null)
        {
            spawner.StartSpawning();
            Debug.Log("Spawner started using INSPECTOR VALUES");
        }
    }


    // ======================
    //        ENDING
    // ======================
    private void OnTimeUp()
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
        timerRunning = false;
        playerAlive = false;

        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ======================
    //        UI
    // ======================
    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // ======================
    //        SENSOR
    // ======================
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
