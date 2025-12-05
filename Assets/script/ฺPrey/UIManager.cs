using UnityEngine;
using UnityEngine.UI; // ต้องมีสำหรับ Image
using TMPro;        // ต้องมีสำหรับ TextMeshPro
using System.Collections;
using System.Collections.Generic;
public class UIManager : MonoBehaviour
{
   // *** แก้ไข: ย้ายการประกาศ HashSet เข้าไปในคลาส และเป็น Static เพื่อติดตาม Global ***
    private static HashSet<string> displayedFishNames = new HashSet<string>();

    [System.Serializable]
    public struct FishUIPanel
    {
        public string fishName;
        public GameObject infoPanelObject;
    }

    [Header("Fish UI Database")]
    public FishUIPanel[] fishPanels;

    private Coroutine hideInfoCoroutine;
    private GameObject currentActivePanel = null; 

    void Start()
    {
        // ... (โค้ดปิด Panel ทั้งหมด) ...
        foreach (FishUIPanel panelProfile in fishPanels)
        {
            if (panelProfile.infoPanelObject != null)
            {
                panelProfile.infoPanelObject.SetActive(false);
            }
        }

        // ตรวจสอบให้แน่ใจว่าเกมเริ่มด้วยเวลาปกติ
        Time.timeScale = 1f;
    }

    public void DisplayFishInfoByName(string targetFishName)
    {
        // 1. *** การติดตาม: ตรวจสอบว่าเคยแสดงข้อมูลไปแล้วหรือไม่ ***
        if (displayedFishNames.Contains(targetFishName))
        {
            // ถ้าเคยแสดงแล้ว ให้ข้ามการเปิด UI
            Debug.Log($"Info for {targetFishName} has already been displayed. Skipping UI.");
            return;
        }

        // 2. ปิด Panel เดิม (ถ้ามี)
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            if (hideInfoCoroutine != null)
            {
                StopCoroutine(hideInfoCoroutine);
            }
        }

        // 3. ค้นหา Panel
        foreach (FishUIPanel panelProfile in fishPanels)
        {
            if (panelProfile.fishName.Equals(targetFishName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (panelProfile.infoPanelObject != null)
                {
                    // 4. เปิด Panel และหยุดเวลาเกม
                    panelProfile.infoPanelObject.SetActive(true);
                    currentActivePanel = panelProfile.infoPanelObject;

                    // *** การพักเกม: ตั้งค่า Time.timeScale เป็น 0 ***
                    Time.timeScale = 0f;
                    
                    // 5. เริ่ม Coroutine รอรับ Input
                    hideInfoCoroutine = StartCoroutine(WaitForKeyPressToHide());

                    // 6. *** บันทึกการแสดงผล: เพิ่มชื่อปลาลงในรายการที่แสดงแล้ว ***
                    displayedFishNames.Add(targetFishName);
                    
                    return; 
                }
            }
        }

        Debug.LogWarning($"UI Panel for '{targetFishName}' not found in UIManager database.");
    }

    // Coroutine สำหรับรอรับการกดปุ่มใดๆ เพื่อซ่อน UI
    IEnumerator WaitForKeyPressToHide()
    {
        while (true)
        {
            // ใช้ yield return null เพื่อรอจนกว่าจะถึงเฟรมถัดไป (ทำงานได้แม้ Time.timeScale = 0)
            yield return null;

            // ตรวจสอบว่ามีการกดปุ่มใดๆ บนคีย์บอร์ดหรือไม่
            if (Input.anyKeyDown)
            {
                HideUIAndResumeGame();
                yield break; 
            }
        }
    }

    /// <summary>
    /// ฟังก์ชันสำหรับซ่อน UI และตั้งค่าเวลาเกมให้กลับเป็นปกติ
    /// </summary>
    public void HideUIAndResumeGame()
    {
        // 1. ซ่อน Panel ที่กำลังเปิดอยู่
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }

        // 2. *** การกลับมาเดินเกม: ตั้งค่า Time.timeScale เป็น 1 ***
        Time.timeScale = 1f;

        // 3. หยุด Coroutine
        if (hideInfoCoroutine != null)
        {
            StopCoroutine(hideInfoCoroutine);
            hideInfoCoroutine = null;
        }
    }
}