using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    // พื้นที่สุ่ม spawn
    [SerializeField] float sizeX = 1f;
    [SerializeField] float sizeY = 1f;

    // เวลาหน่วงการ spawn (สุ่มได้)
    [SerializeField] float minCooldown = 2f;
    [SerializeField] float maxCooldown = 5f;

    // อายุของศัตรูหลัง spawn (อยากลบหลังเกิด เช่น 10 วิ)
    [SerializeField] float enemyLifeTime = 10f;

    private float spawnTime;

    void Start()
    {
        spawnTime = Random.Range(minCooldown, maxCooldown);
    }

    void Update()
    {
        if (spawnTime > 0) spawnTime -= Time.deltaTime;

        if (spawnTime <= 0)
        {
            Spawn();
            spawnTime = Random.Range(minCooldown, maxCooldown); // สุ่มใหม่ทุกครั้ง
        }
    }

    void Spawn()
    {
        // สุ่มตำแหน่ง
        float xPos = (Random.value - 0.5f) * 2 * sizeX + transform.position.x;
        float yPos = (Random.value - 0.5f) * 2 * sizeY + transform.position.y;

        // สร้าง enemy
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);

        // ตั้งเวลา auto destroy
        Destroy(enemy, enemyLifeTime);
    }
}
