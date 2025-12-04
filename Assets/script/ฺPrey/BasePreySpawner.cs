using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePreySpawner : MonoBehaviour
{
    // *** NEW/MODIFIED: ตัวแปรเหล่านี้จะถูก Manager กำหนดค่าให้ ***
    public float minSpawnDelay = 1f; // Public เพื่อให้ Manager เข้าถึงได้
    public float maxSpawnDelay = 3f; // Public เพื่อให้ Manager เข้าถึงได้
    private Vector2 currentDirection; // เก็บค่าทิศทางที่ Manager ตั้งให้

    [Header("Prey Settings")]
    public GameObject[] preyPrefabs;

    [Header("Spawn Area")]
    public float sizeX = 1f;
    public float sizeY = 1f;

    [Header("Self-Destruction Timer")]
    public float spawnerLifetime = 0f;

    [Header("Optional Flip Settings")]
    public bool flipSpriteX = false;

    // *** MODIFIED: Start() ***
    void Start()
    {
        // ใช้งาน Self-Destruction Timer
        if (spawnerLifetime > 0f)
        {
            Destroy(gameObject, spawnerLifetime);
        }

        // *** NEW: ไม่ต้องเรียก StartCoroutine ที่นี่ ให้ Manager เป็นคนเรียก StartSpawner() แทน ***
    }

    // *** NEW: ฟังก์ชันที่ Manager ใช้กำหนดทิศทาง ***
    public void SetSpawnDirection(Vector2 direction)
    {
        currentDirection = direction;
        StartSpawner(); // เมื่อ Manager กำหนดทิศทางและค่าอื่น ๆ แล้ว ให้เริ่ม Spawner
    }

    // *** NEW: ฟังก์ชันที่ Manager ใช้เพื่อเปลี่ยน Delay ขณะเล่นเกม ***
    public void UpdateDelay(float newMin, float newMax)
    {
        minSpawnDelay = newMin;
        maxSpawnDelay = newMax;
    }

    // *** NEW: ฟังก์ชันที่ใช้เริ่มต้น Coroutine (ถูกเรียกโดย Manager) ***
    public void StartSpawner()
    {
        if (preyPrefabs == null || preyPrefabs.Length == 0)
        {
            Debug.LogError("Prey Prefabs list is empty on " + gameObject.name);
            enabled = false;
            return;
        }

        StartCoroutine(SpawnPreyRoutine());
    }

    IEnumerator SpawnPreyRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
            Spawn();
        }
    }

    public GameObject Spawn()
    {
        if (preyPrefabs == null || preyPrefabs.Length == 0)
        {
            return null;
        }

        // 1. สุ่มเลือก Prefab จากลิสต์
        int randomIndex = Random.Range(0, preyPrefabs.Length);
        GameObject preyPrefab = preyPrefabs[randomIndex];

        // 2. กำหนดตำแหน่งเกิดหลัก (Base Spawn Position)
        float randomX = Random.Range(-sizeX, sizeX);
        float randomY = Random.Range(-sizeY, sizeY);
        // เปลี่ยนชื่อตัวแปรเป็น baseSpawnPosition เพื่อใช้เป็นจุดอ้างอิงของฝูง
        Vector3 baseSpawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        // === NEW: ตรวจสอบและตั้งค่าฝูง ===
        SchoolingFishComponent schoolComponent = preyPrefab.GetComponent<SchoolingFishComponent>();

        int spawnCount = 1;
        float scatterRadius = 0f;

        if (schoolComponent != null)
        {
            // *** แก้ไข: ใช้ Random.Range เพื่อสุ่มจำนวนฝูงระหว่าง Min และ Max ***
            // (เราใช้ Random.Range แบบ int โดยเพิ่ม 1 ในค่าสูงสุดเพื่อให้ 30 ถูกรวมอยู่ในการสุ่มด้วย)
            spawnCount = Random.Range(schoolComponent.minSchoolSize, schoolComponent.maxSchoolSize + 1);
            scatterRadius = schoolComponent.scatterRadius;
        }
        // ===============================

        GameObject firstSpawnedPrey = null; // ใช้เก็บปลาตัวแรกที่สร้าง

        for (int i = 0; i < spawnCount; i++) // วนลูปตามจำนวนฝูง (ถ้าไม่ใช่ฝูง จะวนแค่ 1 ครั้ง)
        {
            // 3. คำนวณตำแหน่งที่กระจัดกระจายสำหรับปลาแต่ละตัว
            // Random.insideUnitCircle จะสุ่ม Vector2 ภายในรัศมี 1.0
            Vector3 scatterOffset = Random.insideUnitCircle * scatterRadius;
            Vector3 finalSpawnPosition = baseSpawnPosition + scatterOffset;

            // 4. สั่ง Instantiate Enemy
            GameObject enemy = Instantiate(preyPrefab, finalSpawnPosition, Quaternion.identity);

            // เก็บปลาตัวแรกไว้เป็นตัวแทนในการ return
            if (i == 0)
            {
                firstSpawnedPrey = enemy;
            }

            // 5. ตั้งค่า Flip Sprite X
            if (flipSpriteX)
            {
                SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = true;
                }
            }

            // 6. ตั้งค่าทิศทาง
            Prey preyScript = enemy.GetComponent<Prey>();
            if (preyScript != null)
            {
                // ทุกตัวในฝูงจะวิ่งไปในทิศทางเดียวกัน
                preyScript.direction = currentDirection;
            }
        }

        // return ตัวปลาตัวแรกที่ถูกสร้าง
        return firstSpawnedPrey;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.4f, 0.5f);
        Vector3 size = new Vector3(sizeX * 2, sizeY * 2, 0);
        Gizmos.DrawWireCube(transform.position, size);
    }
}