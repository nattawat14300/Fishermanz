using UnityEngine;
using TMPro;

public class PlayerMovementBySensor : MonoBehaviour
{
    [Header("Sensor Input")]
    public ForcePadReader pad;
    public float threshold = 300f;

    [Header("Step Settings")]
    public float stepSize = 0.3f;
    public float smoothTime = 0.08f;

    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;

    private Vector3 baseScale;

    // ใช้ตรวจ 0 → 1 (กันเด้งซ้ำ)
    private bool lastLeft, lastRight, lastUp, lastDown, lastF5;

    void Start()
    {
        targetPos = transform.position;
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (pad == null) return;

        // ---------------- Sensor Logic ----------------
        bool left = pad.f1 > threshold;   // A0 = left
        bool right = pad.f2 > threshold;   // A1 = right
        bool up = pad.f3 > threshold;   // A2 = up

        // A3 และ A4 ต้องกดพร้อมกันถึงจะลง
        bool down = (pad.f4 > threshold) && (pad.f5 > threshold);

        // ------------------------------------------------
        // Movement Step-by-Step (Detect rising edge)
        // ------------------------------------------------

        // Up
        if (up && !lastUp)
            targetPos += Vector3.up * stepSize;

        // Down (ต้องกด A3 + A4)
        if (down && !lastDown)
            targetPos += Vector3.down * stepSize;

        // Left
        if (left && !lastLeft)
        {
            targetPos += Vector3.left * stepSize;

            transform.localScale = new Vector3(
                Mathf.Abs(baseScale.x),
                baseScale.y,
                baseScale.z
            );
        }

        // Right
        if (right && !lastRight)
        {
            targetPos += Vector3.right * stepSize;

            transform.localScale = new Vector3(
                -Mathf.Abs(baseScale.x),
                baseScale.y,
                baseScale.z
            );
        }

        // Smooth movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );

        // Update last states
        lastLeft = left;
        lastRight = right;
        lastUp = up;
        lastDown = down;
        Debug.Log("Timescale" + Time.timeScale);
    }

}
