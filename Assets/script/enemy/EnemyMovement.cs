using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement (Inspector)")]
    [SerializeField] private float moveSpeed = 1f;   // ✅ ปรับใน Inspector ได้จริง

    private Vector3 moveDir;   // ✅ ล็อกทิศเหมือนเดิม
    private Rigidbody2D rb;

    void Start()
    {
        // ✅ ดึง Rigidbody
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // ✅ หา Player
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure Player has tag = Player");
            enabled = false;
            return;
        }

        // ✅ ล็อกทิศเหมือนเดิม
        moveDir = (player.position - transform.position).normalized;

        // ✅ Debug ดูค่า speed
        Debug.Log("Enemy speed from Inspector = " + moveSpeed);
    }

    void Update()
    {
        // ❌ ยังไม่เริ่มเกม → ไม่ขยับ
        if (!CountdownTimer.IsGameReady) return;

        // ✅ ใช้ค่า Inspector 100%
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    // ======================
    //      DAMAGE
    // ======================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(1);
        }
    }

    // ======================
    //     DESTROY
    // ======================
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            StartCoroutine(DestroyNextFrame());
        }
    }

    IEnumerator DestroyNextFrame()
    {
        yield return null;
        Destroy(gameObject);
    }
}
