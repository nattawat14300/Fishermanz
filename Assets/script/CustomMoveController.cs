using UnityEngine;

public class PlayerStepMove : MonoBehaviour
{
    [Header("Step Settings")]
    public float stepSize = 0.3f;
    public float smoothTime = 0.08f;

    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;

    private Vector3 baseScale;

    // ใช้ตรวจ 0 → 1 (กันกดซ้ำ)
    private bool lastLeft, lastRight, lastUp, lastDown;

    void Start()
    {
        targetPos = transform.position;
        baseScale = transform.localScale;
    }

    void Update()
    {
        // ---------------- Keyboard Input ----------------
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // ------------------------------------------------
        // Movement Step-by-Step (Detect rising edge)
        // ------------------------------------------------

        // Up
        if (up && !lastUp)
            targetPos += Vector3.up * stepSize;

        // Down
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

        // Smooth movement (เหมือน Sensor Version)
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
    }
}
