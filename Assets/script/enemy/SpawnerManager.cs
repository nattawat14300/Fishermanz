using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public EnemySpawner[] spawners; // ลาก Spawner ทั้ง 3 จุดใส่ใน Inspector

    [Header("Global Spawn Settings")]
    public float minCooldown = 1f;
    public float maxCooldown = 3f;

    private float spawnTimer = 0f;

    void Start()
    {
        spawnTimer = Random.Range(minCooldown, maxCooldown);
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnFromRandomSpawner();
            spawnTimer = Random.Range(minCooldown, maxCooldown); // รีเซ็ตเวลา
        }
    }

    void SpawnFromRandomSpawner()
    {
        if (spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);  // เลือก spawner แบบสุ่ม
        spawners[index].Spawn();
    }
}
