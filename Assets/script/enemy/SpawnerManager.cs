using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawners")]
<<<<<<< HEAD
    public EnemySpawner[] spawners;   // Assign Spawner ทั้งหมดใน Inspector
=======
    public EnemySpawner[] spawners; // Assign Spawner ทั้งหมดใน Inspector
>>>>>>> parent of 0c3d1d9 (1111)

    [Header("Spawn Timing")]
    public float minCooldown = 1f;
    public float maxCooldown = 3f;

    private float spawnTimer; // ไม่ได้ใช้แล้วแต่ปล่อยไว้ได้
    private Coroutine spawnRoutine;

    void Start()
    {
<<<<<<< HEAD
        // ❌ ไม่ spawn ตอนเริ่มเกม
        StopSpawning();

        if (spawners == null || spawners.Length == 0)
            Debug.LogWarning("No spawners assigned in SpawnerManager!");
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
=======
        if (spawners.Length == 0)
        {
            Debug.LogWarning("No spawners assigned in SpawnerManager!");
            return;
        }
>>>>>>> parent of 0c3d1d9 (1111)
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

<<<<<<< HEAD
    // ✅ เปลี่ยนค่า spawn จากโค้ด (ยังใช้ Inspector เป็นค่าเริ่ม)
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
=======
    /// <summary>
    /// Coroutine ที่ทำงานซ้ำๆ เพื่อ Spawn Enemy
    /// </summary>
    IEnumerator SpawnRoutine(float minCooldown, float maxCooldown)
>>>>>>> parent of 0c3d1d9 (1111)
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

<<<<<<< HEAD
        // กัน null
=======
        // ✅ NEW: ตรวจสอบว่า Spawner ที่สุ่มมาถูกทำลายไปแล้วหรือไม่
>>>>>>> parent of 0c3d1d9 (1111)
        if (spawner == null)
        {
            // ถ้า Spawner ถูกทำลายแล้ว ให้ข้ามการ Spawn ในรอบนี้ไป
            return;
        }

<<<<<<< HEAD
        GameObject enemy = spawner.Spawn();

        // ปรับค่าเพิ่มเติมกับ Enemy ถ้าต้องการ
        if (enemy != null)
        {
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            if (em != null)
            {
                // ตอนนี้ยังไม่เปลี่ยนค่าใด ๆ (ไว้ต่อยอดได้)
=======
        GameObject enemy = spawner.Spawn(); // เรียกใช้งานได้โดยปลอดภัย

        if (enemy != null)
        {
            // ... โค้ดที่เหลือยังคงเดิม
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            if (em != null)
            {
>>>>>>> parent of 0c3d1d9 (1111)
                em.moveSpeed = em.moveSpeed;
            }
        }
    }
}

