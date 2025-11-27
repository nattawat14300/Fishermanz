using UnityEngine;

public class PlayerMovementBySensor : MonoBehaviour
{
    [Header("Sensor Input")]
    public ForcePadReader pad;
    public float threshold = 300f;   // ค่าที่ถือว่า "กดจริง"

    [Header("Step Settings")]
    public float stepSize = 0.3f;
    public float smoothTime = 0.08f;

    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;

    private Vector3 baseScale;

    // ใช้ตรวจว่ากดจาก 0 → 1 (กันเด้งรัว)
    private bool lastF1, lastF2, lastF3, lastF4, lastF5;

    void Start()
    {
        targetPos = transform.position;
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (pad == null) return;

        bool f1 = pad.f1 > threshold;   // LEFT
        bool f2 = pad.f2 > threshold;   // RIGHT
        bool f3 = pad.f3 > threshold;   // UP
        bool f4 = pad.f4 > threshold;   // DOWN
        bool f5 = pad.f5 > threshold;   // ACTION (optional)

        // -------- Movement (Step-by-step) ----------
        if (f3 && !lastF3)   // UP step
            targetPos += Vector3.up * stepSize;

        if (f4 && !lastF4)   // DOWN step
            targetPos += Vector3.down * stepSize;

        if (f1 && !lastF1)   // LEFT step
        {
            targetPos += Vector3.left * stepSize;

            transform.localScale = new Vector3(
                Mathf.Abs(baseScale.x),
                baseScale.y,
                baseScale.z
            );
        }

        if (f2 && !lastF2)   // RIGHT step
        {
            targetPos += Vector3.right * stepSize;

            transform.localScale = new Vector3(
                -Mathf.Abs(baseScale.x),
                baseScale.y,
                baseScale.z
            );
        }

        // Optional Action
        if (f5 && !lastF5)
            Debug.Log("Sensor Action Button!");

        // ---------- Smooth Move ----------
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );

        // ---------- update last states ----------
        lastF1 = f1;
        lastF2 = f2;
        lastF3 = f3;
        lastF4 = f4;
        lastF5 = f5;
    }
}
