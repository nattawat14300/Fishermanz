using UnityEngine;

// สคริปต์นี้ใช้เพียงเก็บข้อมูลการตั้งค่าฝูง
public class SchoolingFishComponent : MonoBehaviour
{
   // *** แก้ไข: เปลี่ยนเป็น Min/Max สำหรับการสุ่มจำนวน ***
    [Tooltip("จำนวนปลาต่ำสุดที่จะเกิดพร้อมกันในฝูง")]
    public int minSchoolSize = 10;
    
    [Tooltip("จำนวนปลาสูงสุดที่จะเกิดพร้อมกันในฝูง")]
    public int maxSchoolSize = 30; // ตอนนี้ค่าสูงสุดคือ 30

    [Tooltip("ระยะห่างสูงสุดในการสุ่มตำแหน่งของปลาแต่ละตัวในฝูง (ทำให้ดูเป็นธรรมชาติ)")]
    public float scatterRadius = 0.5f;
}