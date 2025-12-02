using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StayInScreen : MonoBehaviour
{
    public Camera cam;
    public float skin = 0.01f;
    Collider2D col;
    Vector2 halfExtents;
    float halfWidth, halfHeight;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
        col = GetComponent<Collider2D>();
        UpdateCameraBounds();
        UpdateHalfExtents();
    }

    void UpdateCameraBounds()
    {
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void UpdateHalfExtents()
    {
        var b = col.bounds;
        halfExtents = (Vector2)b.extents;
        halfExtents = Vector2.Max(halfExtents - Vector2.one * skin, Vector2.zero);
    }

    public Vector2 ClampPosition(Vector2 desired)
    {
        Vector3 camPos = cam.transform.position;
        float left = camPos.x - halfWidth + halfExtents.x;
        float right = camPos.x + halfWidth - halfExtents.x;
        float bottom = camPos.y - halfHeight + halfExtents.y;
        float top = camPos.y + halfHeight - halfExtents.y;

        Vector2 clamped = desired;
        clamped.x = Mathf.Clamp(desired.x, left, right);
        clamped.y = Mathf.Clamp(desired.y, bottom, top);
        return clamped;
    }
}
