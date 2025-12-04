using UnityEngine;
using System.Collections;

public class PreySpawner : MonoBehaviour
{
    [Header("Spawner References")]
    [Tooltip("ลาก BasePreySpawner ตัวซ้ายมาใส่")]
    public BasePreySpawner leftSpawner;

    [Tooltip("ลาก BasePreySpawner ตัวขวามาใส่")]
    public BasePreySpawner rightSpawner;

    // === 2. การตั้งค่ากลางสำหรับ Spawner ทั้งหมด ===
    [Header("Global Spawn Settings")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;

    // *** NEW: ตัวแปรสำหรับการกำหนดทิศทางของแต่ละ Spawner โดย Manager ***
    [Header("Individual Direction Settings")]
    public Vector2 leftSpawnerDirection = Vector2.right;
    public Vector2 rightSpawnerDirection = Vector2.left;


    void Start()
    {
        InitializeSpawners();
    }

    /// <summary>
    /// กำหนดค่าเริ่มต้นและทิศทางให้กับ Spawner ทั้งสองตัว
    /// </summary>
    void InitializeSpawners()
    {
        if (leftSpawner != null)
        {
            // กำหนดช่วงเวลาสปอน
            leftSpawner.minSpawnDelay = minSpawnDelay;
            leftSpawner.maxSpawnDelay = maxSpawnDelay;

            // กำหนดทิศทางการวิ่งออกจาก Spawner ซ้าย (วิ่งไปทางขวา)
            leftSpawner.SetSpawnDirection(leftSpawnerDirection);

            Debug.Log("Left Spawner Initialized.");
        }

        if (rightSpawner != null)
        {
            // กำหนดช่วงเวลาสปอน
            rightSpawner.minSpawnDelay = minSpawnDelay;
            rightSpawner.maxSpawnDelay = maxSpawnDelay;

            // กำหนดทิศทางการวิ่งออกจาก Spawner ขวา (วิ่งไปทางซ้าย)
            rightSpawner.SetSpawnDirection(rightSpawnerDirection);

            Debug.Log("Right Spawner Initialized.");
        }

        // *หลังจากนี้ Spawner จะเริ่มทำงานเองทันทีที่ถูก Initialize*
    }

    /// <summary>
    /// ฟังก์ชันสำหรับเรียกจากที่อื่นเพื่อเปลี่ยนความเร็วในการสปอน
    /// </summary>
    public void UpdateSpawnSpeed(float newMinDelay, float newMaxDelay)
    {
        minSpawnDelay = newMinDelay;
        maxSpawnDelay = newMaxDelay;

        if (leftSpawner != null)
            leftSpawner.UpdateDelay(newMinDelay, newMaxDelay);

        if (rightSpawner != null)
            rightSpawner.UpdateDelay(newMinDelay, newMaxDelay);

        Debug.Log($"Spawn rate updated to {newMinDelay}-{newMaxDelay} seconds.");
    }
}