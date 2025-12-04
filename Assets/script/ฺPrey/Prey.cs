using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    // *** NEW: ตัวแปรคะแนนรวม (Static Global Score) ***
    // ตัวแปรนี้จะเก็บคะแนนรวมทั้งหมด และสามารถเข้าถึงได้จากทุกที่ด้วย Prey.totalScore
    public static int totalScore = 0;

    // === 1. การเคลื่อนที่ ===
    [Tooltip("ความเร็วในการเคลื่อนที่ของเหยื่อ")]
    public float moveSpeed = 5f;

    [Tooltip("ทิศทางในการเคลื่อนที่ (เช่น Vector2.left)")]
    public Vector2 direction = Vector2.left;

    // === 2. สถานะ ===
    private int health = 1;
    private bool hasBeenEatenOnce = false;
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

                // *** แก้ไข: เพิ่มคะแนนเข้าตัวแปร Static โดยตรง ***
                totalScore += 1; // เพิ่ม 1 แต้ม
                Debug.Log($"คะแนนปัจจุบัน: {totalScore}");

                // *** แสดงข้อมูลเมื่อถูกกินครั้งแรก ***
                if (!hasBeenEatenOnce && uiManager != null)
                {
                    uiManager.DisplayFishInfo(fishSpriteImage, fishName, fishDescription);
                    hasBeenEatenOnce = true;
                }

                // *** ตั้งค่า Flag ว่าถูกเก็บแล้ว ***
                isCollected = true;

                // หายไปจากฉาก
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
