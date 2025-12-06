using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawners")]
    public EnemySpawner[] spawners;

    [Header("Spawn Timing (Inspector Controls)")]
    public float minCooldown = 2f;
    public float maxCooldown = 5f;

    private Coroutine spawnRoutine;
    private bool isSpawning = false;

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

            SpawnFromRandomSpawner();
        }
    }

    void SpawnFromRandomSpawner()
    {
        if (spawners == null || spawners.Length == 0) return;

        int index = Random.Range(0, spawners.Length);
        EnemySpawner spawner = spawners[index];

        if (spawner == null)
        {
            Debug.LogWarning("Spawner is null, skipping spawn.");
            return;
        }

        spawner.Spawn();
    }
}
