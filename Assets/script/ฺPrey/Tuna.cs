using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tuna : MonoBehaviour
{

    [Tooltip("ความเร็วในการเคลื่อนที่ของเหยื่อ")]
    public float moveSpeed = 5f;

    [Tooltip("ทิศทางในการเคลื่อนที่ (Vector2.left หรือ Vector2.right เป็นต้น)")]
    public Vector2 direction = Vector2.left; // กำหนดให้เคลื่อนที่ไปทางซ้ายเป็นค่าเริ่มต้น

    private int health = 1;
    private bool hasBeenEatenOnce = false;
    private bool isCollec = false;

    // *** เพิ่มตัวแปรสำหรับรูปภาพ (Sprite) ***
    public Sprite fishSpriteImage; 
    
    // ข้อมูลของปลาที่จะแสดงเมื่อถูกกินครั้งแรก
    public string fishName = "Tuna";
    [TextArea]
    public string fishDescription = "ปลานี้มีประโยชน์ต่อผู้เล่นมาก!";

    // *** อ้างอิงถึง UIManager ***
    private UIManager uiManager;

    void Start()
    {
        // ค้นหา UIManager ในฉากเมื่อเริ่มเกม
        uiManager = FindObjectOfType<UIManager>();
    }

    // ฟังก์ชันที่ถูกเรียกเมื่อ Collider เข้าสู่ Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (health > 0)
            {
                // ... ส่วนลดเลือดและเพิ่มแต้ม ... 
                // (โค้ดส่วนนี้ยังเหมือนเดิม ดูในคำตอบก่อนหน้า)
                
                // ลดเลือด
                health--;

                // *** เปลี่ยนมาเรียก UIManager แทนการ Debug.Log ***
                if (!hasBeenEatenOnce && uiManager != null)
                {
                    // ส่งข้อมูล ชื่อ รายละเอียด และรูปภาพ ไปให้ UIManager แสดงผล
                    uiManager.DisplayFishInfo(fishSpriteImage, fishName, fishDescription);
                    hasBeenEatenOnce = true;
                }

                Debug.Log($"{fishName} is Collected");
                Destroy(gameObject);

                isCollec = true;
            }
        }
    }

    void Update()
    {
        // เคลื่อนที่วัตถุตามทิศทางและด้วยความเร็วที่กำหนด
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }


    private void OnBecameInvisible()
    {
        if(isCollec = false){
        Debug.Log($"**{fishName}** หลุดออกจากฉากและถูกทำลาย");
        Destroy(gameObject);
        }
    }
}
