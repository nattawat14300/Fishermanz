using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

      public float moveSpeed = 3f;

    private Vector3 moveDir;
    private bool hasLockedDirection = false;

    void Start()
    {
        // หา Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure Player has tag = Player");
            return;
        }

        // ✅ ล็อกทิศทางไปยังตำแหน่ง Player แค่ครั้งเดียว
        moveDir = (player.transform.position - transform.position).normalized;
        hasLockedDirection = true;
    }

    void Update()
    {
        if (!hasLockedDirection) return;

        // ✅ เคลื่อนที่ผ่านตำแหน่ง player ต่อไปเรื่อย ๆ
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // ✅ ถ้าออกนอกจอ → ทำลายตัวเอง
  
}

