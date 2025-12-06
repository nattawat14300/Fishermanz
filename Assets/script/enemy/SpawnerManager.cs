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
        // ❌ ไม่ให้ spawn ตอนเริ่มเกม
        StopSpawning();
    }

    // =========================
    //       PUBLIC API
    // =========================

    // ✅ เรียกจาก CountdownTimer ตอนกด NEXT
    public void StartSpawning()
    {
        if (isSpawning) return;   // กันซ้ำ

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

    // ✅ ถ้าอยากปรับผ่านโค้ด ก็ได้ (แต่ยังใช้ Inspector default)
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
            // ✅ ใช้ค่าที่ตั้งใน Inspector
            float delay = Random.Range(minCooldown, maxCooldown);
<<<<<<< Updated upstream

            // ✅ ไม่โดน Time.timeScale = 0
            yield return new WaitForSecondsRealtime(delay);

=======
            yield return new WaitForSecondsRealtime(delay); // ✅ ใช้ Realtime
>>>>>>> Stashed changes
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
    public void StartSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        // เริ่ม Coroutine ใหม่ด้วยค่า cooldown ปัจจุบัน
        spawnRoutine = StartCoroutine(SpawnRoutine(minCooldown, maxCooldown));

        Debug.Log("SpawnerManager: Spawning started.");
    }
}
