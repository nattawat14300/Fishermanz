using System.Collections.Generic;
using UnityEngine;

public class OrcaDamage : MonoBehaviour
{
    public int damage = 1;
    private HashSet<GameObject> alreadyHit = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = other.transform.root.gameObject;

        if (player.CompareTag("Player") && !alreadyHit.Contains(player))
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                alreadyHit.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject player = other.transform.root.gameObject;

        if (player.CompareTag("Player"))
        {
            alreadyHit.Remove(player);
        }
    }

}
