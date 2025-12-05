using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    // *** NEW: ตัวแปรคะแนนรวม (Static Global Score) ***
    public static int totalScore = 0;

    // === 1. การเคลื่อนที่ ===
    public float moveSpeed = 5f;
    public Vector2 direction = Vector2.left;

    // === 2. สถานะ ===
    private int health = 1;
    // *** OLD: private bool hasBeenEatenOnce = false; (ลบทิ้ง) ***
    private bool isCollected = false;

    // === 3. ข้อมูลปลาสำหรับ UI ===
    public Sprite fishSpriteImage;
    public string fishName = "Prey";
    [TextArea]
    public string fishDescription = "รายละเอียดเหยื่อเริ่มต้น";

    // === 4. การอ้างอิง ===
    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError($"UIManager is not found! {fishName}'s info will not display.");
        }
    }

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (health > 0)
            {
                health--;
                totalScore += 1; 
                Debug.Log($"คะแนนปัจจุบัน: {totalScore}");

                // *** แสดงข้อมูลปลา (UIManager จะจัดการการตรวจสอบซ้ำเอง) ***
                if (uiManager != null)
                {
                    uiManager.DisplayFishInfoByName(fishName);
                }

                isCollected = true;
                Die();
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (!isCollected)
        {
            Debug.Log($"**{fishName}** หลุดออกจากฉากและถูกทำลาย");
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log($"**{fishName}** ถูกเก็บแล้ว!");
    }
}
