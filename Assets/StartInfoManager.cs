using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInfoManager : MonoBehaviour
{
    public GameObject[] infoPanels;
    private int currentIndex = 0;
    private bool introActive = true;

    [Header("Sensor Input")]
    public ForcePadReader pad;
    public float threshold = 300f;
    private bool sensorConsumed = false;

    public void Awake()
    {
        Time.timeScale = 0f;
    }

    void Start()
    {
        // ปิดทุก panel
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(false);

        // เปิดหน้าแรก
        ShowPanel(0);
        CountdownTimer.IsGameReady = false;

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!introActive) return;

        bool anyKey = Input.anyKeyDown || Input.GetMouseButtonDown(0);
        bool anySensor = IsAnySensorPressed();

        // กันกดรัว: sensor ต้อง "ยกนิ้ว" ก่อน
        if (!anySensor)
            sensorConsumed = false;

        if ((anyKey || anySensor) && !sensorConsumed)
        {
            sensorConsumed = true;

            if (currentIndex >= infoPanels.Length - 1)
                PlayGame();
            else
                NextInfo();
        }
    }

    bool IsAnySensorPressed()
    {
        if (pad == null) return false;

        return pad.f1 > threshold ||
               pad.f2 > threshold ||
               pad.f3 > threshold ||
               pad.f4 > threshold ||
               pad.f5 > threshold;
    }

    public void NextInfo()
    {
        currentIndex++;
        ShowPanel(currentIndex);
    }

    public void PlayGame()
    {
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(false);

        Time.timeScale = 1f;
        introActive = false;

        CountdownTimer.IsGameReady = true;

        Debug.Log("Game Started!");
    }

    void ShowPanel(int index)
    {
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(i == index);
    }
}
