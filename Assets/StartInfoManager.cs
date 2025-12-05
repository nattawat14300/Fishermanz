using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInfoManager : MonoBehaviour
{
    public GameObject[] infoPanels;
    private int currentIndex = 0;
    private bool introActive = true;

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

        // หยุดเกม
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!introActive) return;

        // ✅ กดปุ่มใดก็ได้ = Next
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            // ถ้าอยู่หน้า Panel สุดท้าย → Play
            if (currentIndex >= infoPanels.Length - 1)
            {
                PlayGame();
            }
            else
            {
                NextInfo();
            }
        }
    }

    public void NextInfo()
    {
        currentIndex++;
        ShowPanel(currentIndex);
    }

    public void PlayGame()
    {
        // ปิด Panel ทั้งหมด
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
