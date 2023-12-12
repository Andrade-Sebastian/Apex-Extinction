using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float explosionRadius = 3f;
    public float explosionForce = 10f;
    public float fuseDuration = 3f;
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public AudioClip explosionSound;
    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Start the fuse countdown
        Invoke("Explode", fuseDuration);
    }

    void Explode()
    {

        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Instantiate the explosion prefab
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Play the explosion animation
        Animator explosionAnimator = explosion.GetComponent<Animator>();
        if (explosionAnimator != null)
        {
            explosionAnimator.Play("ExplosionAnimation");
        }

        // Handle explosion logic (e.g., deal damage, apply force)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            // Apply explosive force to objects within the explosion radius
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }

            // Add additional logic as needed, such as damaging the player
            if (col.CompareTag("Player"))
            {
                PlayerStats playerStats = col.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(30); // Adjust the damage value as needed
                }
            }
        }

        // Destroy the bomb and the explosion after a delay
        Destroy(explosion, 1.0f); // Adjust the delay based on the explosion animation duration
        Destroy(gameObject);
    }
}
