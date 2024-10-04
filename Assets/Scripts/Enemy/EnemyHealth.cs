using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    private Animator anim;
    private AudioSource enemyAudio;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
    private bool isDead = false;
    private bool isSinking = false;

    private Rigidbody rb;  // Cached Rigidbody for sinking optimization
    private UnityEngine.AI.NavMeshAgent navMeshAgent; // Cached NavMeshAgent

    void Awake()
    {
        // Cache components to avoid repeated GetComponent calls
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Initialize health
        currentHealth = startingHealth;
    }

    void Update()
    {
        // Move the enemy down if it is sinking
        if (isSinking)
        {
            // Apply sinking only while the enemy is still active
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        // If the enemy is dead, no further damage can be applied
        if (isDead) return;

        // Play the damage sound only if it's not already playing
        if (!enemyAudio.isPlaying)
        {
            enemyAudio.Play();
        }

        // Reduce health and play hit particles
        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        // Check if health falls to zero and trigger death if so
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // Set the enemy to dead and disable further collision detection
        isDead = true;

        // Set the capsule collider to trigger so that enemies don't block
        capsuleCollider.isTrigger = true;

        // Play the death animation and audio
        anim.SetTrigger("Dead");

        // Swap to death audio clip if available
        if (deathClip != null)
        {
            enemyAudio.clip = deathClip;
            enemyAudio.Play();
        }
    }

    public void StartSinking()
    {
        // Disable the enemy's AI and physics to start sinking
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Set sinking to true, allowing movement downward
        isSinking = true;

        // Update the score
        ScoreManager.score += scoreValue;

        // Destroy the game object after 2 seconds, no need for continuous sinking checks beyond that
        Destroy(gameObject, 2f);
    }
}
