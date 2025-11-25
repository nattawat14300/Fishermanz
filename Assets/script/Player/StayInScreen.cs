using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInScreen : MonoBehaviour
{
    Camera cam;
    float halfWidth, halfHeight;

    void Start()
    {
        cam = Camera.main;

        // คำนวณขนาดครึ่งจอแบบ Orthographic
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // ล็อกแกน X
        pos.x = Mathf.Clamp(
            pos.x,
            cam.transform.position.x - halfWidth,
            cam.transform.position.x + halfWidth
        );

        // ล็อกแกน Y
        pos.y = Mathf.Clamp(
            pos.y,
            cam.transform.position.y - halfHeight,
            cam.transform.position.y + halfHeight
        );

        transform.position = pos;
    }
}
