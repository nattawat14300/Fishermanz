using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab; // Assign Prefab Asset

    [Header("Spawn Area")]
    public float sizeX = 1f;
    public float sizeY = 1f;
    [Header("Self-Destruction Timer")]
    public float spawnerLifetime = 0f;
    [Header("Optional Flip Settings")]
    public bool flipSpriteX = false;

    void Start()
    {
        // ใช้งาน Self-Destruction Timer
        if (spawnerLifetime > 0f)
        {
            Destroy(gameObject, spawnerLifetime);
        }
    }

    // Spawn Enemy และ return GameObject
    public GameObject Spawn()
    {
        if (enemyPrefab == null) return null;

        // 1. ✅ แก้ไข: กำหนดตำแหน่งเกิด (ใช้ตำแหน่ง Spawner ตรงๆ ตามที่คุณต้องการก่อนหน้า)
        Vector3 spawnPosition = transform.position;

        // 2. ✅ แก้ไข: สั่ง Instantiate Enemy และเก็บผลลัพธ์ไว้ในตัวแปร 'enemy'
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // 3. ✅ แก้ไข: ย้ายโค้ด Flip Sprite มาทำงานบน GameObject ที่ถูกสร้าง (enemy)
        if (flipSpriteX)
        {
            // ต้องเรียก GetComponent บนตัว 'enemy' ที่เพิ่งสร้าง
            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = true;
            }
        }

        return enemy;
    }

    void OnDrawGizmosSelected()
    {
        // วาดสี่เหลี่ยมเพื่อแสดงขอบเขตการเกิดของศัตรู
        Gizmos.color = new Color(1f, 0.4f, 0.4f, 0.5f); // สีชมพูใส
        Vector3 size = new Vector3(sizeX * 2, sizeY * 2, 0); // ขนาดรวมคือ 2 * sizeX และ 2 * sizeY
        Gizmos.DrawWireCube(transform.position, size);
    }
}