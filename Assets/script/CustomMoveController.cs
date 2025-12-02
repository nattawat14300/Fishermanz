using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerStepMove : MonoBehaviour
{
    [Header("Step / Smooth")]
    public float stepSize = 1f;            // ระยะต่อก้าว (WASD)
    public float smoothTime = 0.10f;      // SmoothDamp เวลา (0.05-0.15 แนะนำ)

    [Header("Axis (continuous)")]
    public float moveSpeed = 5f;          // ความเร็วเมื่อใช้ Horizontal axis

    // internal
    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;
    private float arriveThreshold = 0.01f;
    private Vector3 baseScale;
    private bool isStepping = false;

    // bounds
    private Camera cam;
    private float halfWidthWorld;
    private float halfHeightWorld;
    private float spriteHalfWidth;
    private float spriteHalfHeight;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found, using default bounds.");
            cam = new GameObject("TempCamera").AddComponent<Camera>();
            cam.orthographic = true;
            cam.transform.position = new Vector3(0, 0, -10);
        }

        targetPos = transform.position;
        baseScale = transform.localScale;

        var sr = GetComponent<SpriteRenderer>();
        spriteHalfWidth = sr ? sr.bounds.extents.x : 0.5f;
        spriteHalfHeight = sr ? sr.bounds.extents.y : 0.5f;

        halfHeightWorld = cam.orthographicSize;
        halfWidthWorld = halfHeightWorld * cam.aspect;

        Time.timeScale = 1f; // เริ่มเกมปกติ
    }

    void Update()
    {
      

        RecalcCamBounds();
        HandleStepInput();
        HandleAxisInput();
        SmoothMoveAndClamp();
    }

    void RecalcCamBounds()
    {
        halfHeightWorld = cam.orthographicSize;
        halfWidthWorld = halfHeightWorld * cam.aspect;
    }

    void HandleStepInput()
    {
        // allow new step only when near target
        if (Vector3.Distance(transform.position, targetPos) < arriveThreshold)
        {
            isStepping = false;
            transform.position = targetPos; // snap exactly
            velocity = Vector3.zero;
        }

        if (isStepping) return;

        Vector3 dir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.S)) dir = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.A)) { dir = Vector3.left; FaceLeft(); }
        else if (Input.GetKeyDown(KeyCode.D)) { dir = Vector3.right; FaceRight(); }

        if (dir != Vector3.zero)
        {
            targetPos += dir * stepSize;
            isStepping = true;

            // clamp target immediately so step won't push outside
            targetPos = ClampToCameraBounds(targetPos);
        }
    }

    void HandleAxisInput()
    {
        // read horizontal axis (-1..1). If using keyboard for axis, Input.GetAxis works fine
        float h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(h) > 0.01f)
        {
            // add continuous movement to target (not direct transform) so SmoothDamp handles smoothing
            Vector3 add = Vector3.right * h * moveSpeed * Time.deltaTime;
            targetPos += add;
            // if player is moving by axis, cancel step-blocking so player can immediately move
            isStepping = false;

            // flip sprite based on axis
            if (h > 0.01f) FaceRight();
            else if (h < -0.01f) FaceLeft();

            // clamp target to bounds immediately
            targetPos = ClampToCameraBounds(targetPos);
        }
    }

    void SmoothMoveAndClamp()
    {
        // SmoothDamp toward targetPos
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // final clamp so smoothing never pushes out of bounds
        newPos = ClampToCameraBounds(newPos);

        transform.position = newPos;
    }

    Vector3 ClampToCameraBounds(Vector3 desired)
    {
        Vector3 camPos = cam.transform.position;

        float left = camPos.x - halfWidthWorld + spriteHalfWidth;
        float right = camPos.x + halfWidthWorld - spriteHalfWidth;
        float bottom = camPos.y - halfHeightWorld + spriteHalfHeight;
        float top = camPos.y + halfHeightWorld - spriteHalfHeight;

        Vector3 clamped = desired;
        clamped.x = Mathf.Clamp(desired.x, left, right);
        clamped.y = Mathf.Clamp(desired.y, bottom, top);
        clamped.z = desired.z; // keep z
        return clamped;
    }

    void FaceRight()
    {
        transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    void FaceLeft()
    {
        transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
    }

    // optional: draw camera bounds in editor
    void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;
        RecalcCamBounds();
        Vector3 camPos = Camera.main.transform.position;
        Vector3 size = new Vector3(halfWidthWorld * 2f, halfHeightWorld * 2f, 0f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(camPos.x, camPos.y, transform.position.z), size);
    }
}
