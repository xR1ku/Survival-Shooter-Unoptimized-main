using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;
    private Transform playerTransform;

    void Awake()
    {
        // Cache components for better performance
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();

        // Find player once and cache the reference
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("PlayerMovement script not found in the scene.");
        }
    }

    void Update()
    {
        // Ensure all references are valid
        if (playerTransform == null || playerHealth == null || enemyHealth == null || navMeshAgent == null)
            return;

        // Check health conditions before setting the destination
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }
}
