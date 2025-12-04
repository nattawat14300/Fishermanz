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

        // ... (โค้ดสุ่มเลือก Prefab และตำแหน่งเหมือนเดิม) ...
        int randomIndex = Random.Range(0, preyPrefabs.Length);
        GameObject preyPrefab = preyPrefabs[randomIndex];

        float randomX = Random.Range(-sizeX, sizeX);
        float randomY = Random.Range(-sizeY, sizeY);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        GameObject enemy = Instantiate(preyPrefab, spawnPosition, Quaternion.identity);

        if (flipSpriteX)
        {
            // *** แก้ไข: ใช้ GetComponentInChildren แทน GetComponent ***
            // นี่คือการค้นหา SpriteRenderer ทั้งในวัตถุหลัก (enemy) และวัตถุลูกทั้งหมด
            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();

            if (sr != null)
            {
                sr.flipX = true;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component not found on the spawned object or its children for flipping.");
            }
        }

        // *** ใช้ทิศทางที่ Manager กำหนดให้ ***
        Prey preyScript = enemy.GetComponent<Prey>();
        if (preyScript != null)
        {
            preyScript.direction = currentDirection;
        }

        return enemy;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0.4f, 0.5f);
        Vector3 size = new Vector3(sizeX * 2, sizeY * 2, 0);
        Gizmos.DrawWireCube(transform.position, size);
    }
}