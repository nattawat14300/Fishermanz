using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Area")]
    public float sizeX = 1f;
    public float sizeY = 1f;

    [Header("Enemy Settings")]
    public float enemyLifeTime = 10f; // optional: กำหนด auto destroy หลังเวลา

    public void Spawn()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemyPrefab not assigned in " + gameObject.name);
            return;
        }

        // Random position ภายในพื้นที่
        float xPos = (Random.value - 0.5f) * 2 * sizeX + transform.position.x;
        float yPos = (Random.value - 0.5f) * 2 * sizeY + transform.position.y;

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

        // ถ้าต้องการ auto destroy หลังเวลาที่กำหนด
        if (enemyLifeTime > 0f)
        {
            Destroy(enemy, enemyLifeTime);
        }
    }
}
