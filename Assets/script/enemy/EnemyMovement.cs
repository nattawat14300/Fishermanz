using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDir;  // ล็อกทิศทางตอน spawn
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure Player has tag = Player");
            return;
        }

        // ล็อกทิศทางไปยังตำแหน่ง Player ตอน spawn
        moveDir = (player.position - transform.position).normalized;
    }

    void Update()
    {
        // เดินไปในทิศทางที่ล็อกไว้
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy ชน Player → ทำ damage แต่ไม่หาย
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // ออกนอก Boundary → ทำลายตัวเอง
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
