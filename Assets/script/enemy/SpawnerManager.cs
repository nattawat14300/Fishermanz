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
        StopSpawning(); // ❌ ไม่ spawn ตอนเริ่ม
    }

    // =========================
    //       PUBLIC API
    // =========================
    public void StartSpawning()
    {
        if (isSpawning) return;

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
            yield return new WaitForSecondsRealtime(delay); // ใช้ Realtime
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
