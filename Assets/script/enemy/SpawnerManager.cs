using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawners")]
    public EnemySpawner[] spawners; // Assign Spawner ทั้งหมดใน Inspector

    [Header("Spawn Timing")]
    public float minCooldown = 1f;
    public float maxCooldown = 3f;
   
        private float spawnTimer;

    void Start()
    {
        if (spawners.Length == 0)
        {
            Debug.LogWarning("No spawners assigned in SpawnerManager!");
            return;
        }

        spawnTimer = Random.Range(minCooldown, maxCooldown);
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnFromRandomSpawner();
            spawnTimer = Random.Range(minCooldown, maxCooldown);
        }
    }

    void SpawnFromRandomSpawner()
    {
        if (spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);
        EnemySpawner spawner = spawners[index];

        // ✅ NEW: ตรวจสอบว่า Spawner ที่สุ่มมาถูกทำลายไปแล้วหรือไม่
        if (spawner == null)
        {
            // ถ้า Spawner ถูกทำลายแล้ว ให้ข้ามการ Spawn ในรอบนี้ไป
            return;
        }

        GameObject enemy = spawner.Spawn(); // เรียกใช้งานได้โดยปลอดภัย

        if (enemy != null)
        {
            // ... โค้ดที่เหลือยังคงเดิม
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            if (em != null)
            {
                em.moveSpeed = em.moveSpeed;
            }
        }
    }
}

