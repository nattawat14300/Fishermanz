using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float moveSpeed = 3f;
    private Vector3 direction;
    private bool isStopped = false;

    void Start()
    {
        // หาตำแหน่ง Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // คำนวณทิศทางครั้งเดียว
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            // ถ้าไม่เจอ player ก็ให้ไปทางขวาแก้ขัด
            direction = Vector3.right;
        }
    }

    void Update()
    {
        // หยุดการเคลื่อนที่
        if (isStopped) return;

        // เดินทางไปตามทิศเดิมแบบเส้นตรง
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    // ให้ Player เรียกใช้ได้
    public void StopMove(bool stop)
    {
        isStopped = stop;
    }
}
