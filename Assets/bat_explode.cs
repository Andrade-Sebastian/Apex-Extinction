using System.Collections;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float explosionForce = 10f;
    public float explosionRadius = 5f;
    public float explosionDuration = 2f; // Adjust this value to set the duration of the explosion.
    public GameObject explosionPrefab;

    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            Explode();
        }
    }

  void Explode()
{
  
    GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

   
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    foreach (Collider2D col in colliders)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (rb.transform.position - transform.position).normalized;
            rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
        }

        // Check if the collider belongs to the player and apply damage
        if (col.CompareTag("Player"))
        {
            PlayerStats playerStats = col.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(30); // Adjust the damage value as needed
            }
        }
    }

   
    StartCoroutine(DestroyExplosion(explosion));
    Destroy(explosion, 1.0f);
  
    Destroy(gameObject);
}


    IEnumerator DestroyExplosion(GameObject explosion)
    {
        yield return new WaitForSeconds(explosionDuration);
        Destroy(explosion);
    }
}