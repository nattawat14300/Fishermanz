using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClampPlayerToScreen : MonoBehaviour
{
    [Header("Camera & Padding")]
    public Camera cam;             // ถ้าว่าง จะใช้ Camera.main
    public Vector2 padding = new Vector2(0.1f, 0.1f); // ระยะเว้นขอบ

    private Collider2D col;
    private Vector2 halfExtents;
    private float halfWidth, halfHeight;

    void Awake()
    {
        if (cam == null) cam = Camera.main;

        col = GetComponent<Collider2D>();
        UpdateHalfExtents();
        UpdateCameraBounds();
    }

    void Update()
    {
        UpdateCameraBounds();
        Vector3 clampedPos = ClampPosition(transform.position);
        transform.position = clampedPos;
    }

    void UpdateHalfExtents()
    {
        var b = col.bounds;
        halfExtents = (Vector2)b.extents;
    }

    void UpdateCameraBounds()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    public Vector3 ClampPosition(Vector3 desired)
    {
        Vector3 camPos = cam.transform.position;

        float left = camPos.x - halfWidth + halfExtents.x + padding.x;
        float right = camPos.x + halfWidth - halfExtents.x - padding.x;
        float bottom = camPos.y - halfHeight + halfExtents.y + padding.y;
        float top = camPos.y + halfHeight - halfExtents.y - padding.y;

        float clampedX = Mathf.Clamp(desired.x, left, right);
        float clampedY = Mathf.Clamp(desired.y, bottom, top);

        return new Vector3(clampedX, clampedY, desired.z);
    }
}
