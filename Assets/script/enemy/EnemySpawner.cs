using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
    
{
    [SerializeField] GameObject enemyPrefab;

    // พื้นที่สุ่ม spawn
    [SerializeField] float sizeX = 1f;
    [SerializeField] float sizeY = 1f;

    // อายุศัตรูหลังเกิด
    [SerializeField] float enemyLifeTime = 10f;

    // Manager จะเรียกใช้

    public int spawnerID = 1;
    public void Spawn()
    {
        float xPos = (Random.value - 0.5f) * 2 * sizeX + transform.position.x;
        float yPos = (Random.value - 0.5f) * 2 * sizeY + transform.position.y;

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        Destroy(enemy, enemyLifeTime);

        if (spawnerID == 2)
        {
            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = true; // หรือ false แล้วแต่สไปรต์
            }
        }

        if (spawnerID == 4)
        {
            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = true; // หรือ false แล้วแต่สไปรต์
            }
        }

        Destroy(enemy, enemyLifeTime);
    }
}

