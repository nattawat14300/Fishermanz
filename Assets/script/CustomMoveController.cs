using UnityEngine;

public class PlayerMovementSmooth : MonoBehaviour
{
    [Header("Step Settings")]
    public float stepSize = 0.3f;
    public float smoothTime = 0.08f;

    [Header("Custom Keys")]
    public KeyCode keyUp = KeyCode.W;
    public KeyCode keyDown = KeyCode.S;
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyRight = KeyCode.D;

    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;

    // เก็บscaleตามที่ตั้งไว้จริงใน Inspector
    private Vector3 baseScale;

    void Start()
    {
        targetPos = transform.position;

        // บันทึกสเกลที่ตั้งไว้ใน Inspector (สำคัญ!)
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyUp))
            targetPos += Vector3.up * stepSize;

        if (Input.GetKeyDown(keyDown))
            targetPos += Vector3.down * stepSize;

        // Left
        if (Input.GetKeyDown(keyLeft))
        {
            targetPos += Vector3.left * stepSize;

            // กลับด้านซ้าย โดยใช้ scale จาก Inspector
            transform.localScale = new Vector3(
                Mathf.Abs(baseScale.x),
                baseScale.y,
                baseScale.z
            );
        }

        // Right
        if (Input.GetKeyDown(keyRight))
        {
            targetPos += Vector3.right * stepSize;

            // กลับด้านขวา โดยใช้ scale เดิม
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
    }
}
