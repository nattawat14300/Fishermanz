using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawners")]
    public EnemySpawner[] spawners;   // Assign Spawner ทั้งหมดใน Inspector

    [Header("Spawn Timing")]
    public float minCooldown = 1f;
    public float maxCooldown = 3f;

    private Coroutine spawnRoutine;
    private bool isSpawning = false;

    void Start()
    {
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
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        isSpawning = false;
        Debug.Log("SpawnerManager: STOP spawning");
    }

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
    {
        while (true)
        {
            float delay = Random.Range(minCooldown, maxCooldown);

            // ✅ ไม่โดน Time.timeScale = 0
            yield return new WaitForSecondsRealtime(delay);

            SpawnFromRandomSpawner();
        }
    }

    void SpawnFromRandomSpawner()
    {
        if (spawners == null || spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);
        EnemySpawner spawner = spawners[index];

        // กัน null
        if (spawner == null)
        {
            Debug.LogWarning("Spawner is null, skipping spawn.");
            return;
        }

        GameObject enemy = spawner.Spawn();

        // ปรับค่าเพิ่มเติมกับ Enemy ถ้าต้องการ
        if (enemy != null)
        {
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            if (em != null)
            {
                // ตอนนี้ยังไม่เปลี่ยนค่าใด ๆ (ไว้ต่อยอดได้)
                em.moveSpeed = em.moveSpeed;
            }
        }
    }
}
