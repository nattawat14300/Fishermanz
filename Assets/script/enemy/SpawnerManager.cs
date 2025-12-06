using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    // =========================
    //         SPAWNERS
    // =========================
    [Header("Spawners")]
    public EnemySpawner[] spawners;

    // =========================
    //       SPAWN TIMING
    // =========================
    [Header("Spawn Timing (Inspector)")]
    public float minCooldown = 1f;
    public float maxCooldown = 3f;

    // =========================
    //         STATE
    // =========================
    private Coroutine spawnRoutine;
    private bool isSpawning = false;

    // =========================
    //          START
    // =========================
    void Start()
    {
        StopSpawning();   // ❌ ไม่ spawn ตอนเริ่มเกม
    }

    // =========================
    //       PUBLIC API
    // =========================
    public void StartSpawning()
    {
        if (isSpawning) return;

        if (spawners == null || spawners.Length == 0)
        {
            Debug.LogWarning("SpawnerManager: No spawners assigned!");
            return;
        }

        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnRoutine());

        Debug.Log("SpawnerManager: START spawning");
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        isSpawning = false;
    }

    public void RestartSpawning()
    {
        StopSpawning();
        StartSpawning();
    }

    public void ChangeSpawnRate(float newMin, float newMax)
    {
        minCooldown = newMin;
        maxCooldown = newMax;

        RestartSpawning();
    }

    // =========================
    //        SPAWN LOOP
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

    // =========================
    //        SPAWN
    // =========================
    void SpawnFromRandomSpawner()
    {
        if (spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);
        EnemySpawner spawner = spawners[index];

        if (spawner == null) return;

        GameObject enemy = spawner.Spawn();

        if (enemy != null)
        {
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            if (em != null)
            {
                em.moveSpeed = em.moveSpeed;   // ใช้ค่าที่ตั้งจาก Prefab
            }
        }
    }
}
