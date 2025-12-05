using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInfoManager : MonoBehaviour
{
    public GameObject[] infoPanels; // ใส่ 3 panel ตามลำดับ
    private int currentIndex = 0;

    public void Awake()
    {
        Time.timeScale = 0f;
    }

    void Start()
    {
        // ปิดทุก panel ก่อน
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(false);

        // เปิดหน้าแรก
        ShowPanel(0);
        CountdownTimer.IsGameReady = false;
    }

    public void NextInfo()
    {
        currentIndex++;

        if (currentIndex < infoPanels.Length)
        {
            ShowPanel(currentIndex);
        }
    }

    public void PlayGame()
    {
        // ปิดทุก panel
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(false);

        // ให้เกมเดิน
        Time.timeScale = 1f;

        CountdownTimer.IsGameReady = true;

        Debug.Log("Game Started!");
    }

    void ShowPanel(int index)
    {
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].SetActive(i == index);
    }
}
