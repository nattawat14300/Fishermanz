using UnityEngine;
using UnityEngine.UI; // ต้องมีสำหรับ Image
using TMPro;        // ต้องมีสำหรับ TextMeshPro

public class UIManager : MonoBehaviour
{
    // อ้างอิงถึง UI Panel/Game Object ที่จะเปิดปิด
    public GameObject fishInfoPanel; 

    // อ้างอิงถึงส่วนประกอบ UI ภายใน Panel
    public Image fishImageUI;
    public TextMeshProUGUI fishNameTextUI;
    public TextMeshProUGUI fishDescriptionTextUI;

    // (ทางเลือก) กำหนดเวลาที่จะให้ Panel ปิดเอง
    public float displayTime = 4f;
    private Coroutine hideInfoCoroutine;

    void Start()
    {
        // ตรวจสอบให้แน่ใจว่า Panel ปิดอยู่เมื่อเริ่มเกม
        if (fishInfoPanel != null)
        {
            fishInfoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// ฟังก์ชันหลักสำหรับแสดงข้อมูลปลา
    /// </summary>
    /// <param name="fishSprite">รูปภาพ (Sprite) ของปลา</param>
    /// <param name="fishName">ชื่อปลา</param>
    /// <param name="fishDescription">รายละเอียดปลา</param>
    public void DisplayFishInfo(Sprite fishSprite, string fishName, string fishDescription)
    {
        // 1. อัพเดทข้อมูล UI
        if (fishImageUI != null)
        {
            fishImageUI.sprite = fishSprite;
        }
        if (fishNameTextUI != null)
        {
            fishNameTextUI.text = fishName;
        }
        if (fishDescriptionTextUI != null)
        {
            fishDescriptionTextUI.text = fishDescription;
        }

        // 2. เปิด Panel
        if (fishInfoPanel != null)
        {
            fishInfoPanel.SetActive(true);
        }

        // 3. เริ่ม Coroutine เพื่อปิด Panel หลังผ่านไป displayTime วินาที
        if (hideInfoCoroutine != null)
        {
            StopCoroutine(hideInfoCoroutine);
        }
        hideInfoCoroutine = StartCoroutine(HideFishInfoAfterDelay());
    }

    // Coroutine สำหรับนับเวลาและปิด UI
    System.Collections.IEnumerator HideFishInfoAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (fishInfoPanel != null)
        {
            fishInfoPanel.SetActive(false);
        }
    }
}