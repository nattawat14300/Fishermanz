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

    private float spawnTimer; // ไม่ได้ใช้แล้วแต่ปล่อยไว้ได้
    private Coroutine spawnRoutine;

    void Start()
    {

        // ❌ ไม่ spawn ตอนเริ่มเกม
        StopSpawning();
    }

    // =========================
    //       PUBLIC API
    // =========================

    // ✅ เรียกจาก CountdownTimer ตอนกด NEXT
    public void StartSpawning()
    {
        if (isSpawning) return;   // กัน start ซ้ำ

        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnRoutine());

        Debug.Log("SpawnerManager: START spawning (Inspector values)");

        if (spawners.Length == 0)
        {
            Debug.LogWarning("No spawners assigned in SpawnerManager!");
            return;
        }

    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
            Debug.Log("SpawnerManager: Spawning routine stopped.");
        }
    }
    public void ChangeSpawnRate(float newMinCooldown, float newMaxCooldown)
    {
        // 1. หยุด Coroutine เก่าก่อน (ถ้ามี)
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        // 2. เริ่ม Coroutine ใหม่ด้วยช่วงเวลาใหม่
        spawnRoutine = StartCoroutine(SpawnRoutine(newMinCooldown, newMaxCooldown));
        Debug.Log($"SpawnerManager: Spawn rate changed to {newMinCooldown}s - {newMaxCooldown}s.");
    }


    // ✅ เผื่ออยากปรับจากโค้ด
    public void ChangeSpawnRate(float newMin, float newMax)
    {
        minCooldown = newMin;
        maxCooldown = newMax;

        RestartSpawning();
    }

    public void RestartSpawning()
    {
        StopSpawning();
        StartSpawning();
    }

    // =========================
    //       SPAWN LOOP
    // =========================

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // ✅ ใช้ค่าจาก Inspector
            float delay = Random.Range(minCooldown, maxCooldown);

            // ✅ ใช้ Realtime → ไม่โดน Time.timeScale = 0
            yield return new WaitForSecondsRealtime(delay);

            /// <summary>
            /// Coroutine ที่ทำงานซ้ำๆ เพื่อ Spawn Enemy
            /// </summary>
            IEnumerator SpawnRoutine(float minCooldown, float maxCooldown)
            {
                while (true)
                {
                    float delay = Random.Range(minCooldown, maxCooldown);
                    yield return new WaitForSeconds(delay);


                    SpawnFromRandomSpawner();
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
    }
}

