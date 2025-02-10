using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewChickenAI : MonoBehaviour
{
    public float movementSpeed = 2f;            // Speed of movement
    public float eggLayInterval = 30f;         // Time interval for laying eggs
    public float foxDetectionRadius = 10f;     // Distance to detect a fox
    public GameObject eggPrefab;               // Egg prefab to spawn
    public LayerMask foxLayer;                 // Layer mask for detecting foxes

    private Animator chickenAnimator;          // Animator for controlling animations
    private NavMeshAgent agent;                // NavMeshAgent for movement
    private float eggTimer;                    // Timer for egg-laying
    private bool isLayingEgg = false;          // Whether the chicken is laying an egg
    private bool isRunningFromFox = false;     // Whether the chicken is running from a fox
    private bool isWandering = false;          // Whether the chicken is wandering

    private ShopManager shopManager;

    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        agent = GetComponent<NavMeshAgent>();
        chickenAnimator = GetComponent<Animator>();
        agent.speed = movementSpeed;

        eggTimer = eggLayInterval; // Initialize egg timer
        StartCoroutine(Wander());
    }

    void Update()
    {
        // Handle egg-laying timer
        if (!isLayingEgg && !isRunningFromFox)
        {
            eggTimer -= Time.deltaTime;
            if (eggTimer <= 0f && !isLayingEgg)
            {
                if (!isLayingEgg)
                {
                    StartCoroutine(LayEgg1());
                }
                eggTimer = eggLayInterval; // Reset timer
                StartCoroutine(Wander());
            }
        }

        // Detect foxes and handle running away
        Collider[] foxes = Physics.OverlapSphere(transform.position, foxDetectionRadius, foxLayer);
        if (foxes.Length > 0)
        {
            RunAway(foxes[0].transform.position);
        }
        else if (isRunningFromFox)
        {
            isRunningFromFox = false;
            chickenAnimator.SetTrigger("stop");
            StartCoroutine(Wander()); // Resume wandering
        }
    }

    IEnumerator LayEgg1()
    {
        isLayingEgg = true;
        chickenAnimator.SetTrigger("lay"); // Trigger "lay" animation

        // Spawn the egg
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f;
        Instantiate(eggPrefab, eggSpawnPosition, Quaternion.identity);

        shopManager.AddEgg();

        chickenAnimator.SetTrigger("stop"); // Transition back to "Idle"
        isLayingEgg = false;

        yield return null; // Ensure the coroutine completes
    }

    void RunAway(Vector3 foxPosition)
    {
        isRunningFromFox = true;
        StopCoroutine(Wander()); // Stop wandering
        chickenAnimator.SetTrigger("walk");

        Vector3 runDirection = (transform.position - foxPosition).normalized * 10f;
        Vector3 targetPosition = transform.position + runDirection;

        agent.SetDestination(targetPosition);
    }

    IEnumerator Wander()
    {
        isWandering = true;

        while (!isLayingEgg && !isRunningFromFox)
        {
            // Choose a random action: idle, walk, peck, or rotate
            int action = Random.Range(0, 4); // 0 = idle, 1 = walk, 2 = rotate, 3 = peck
            if (action == 0) // Idle
            {
                chickenAnimator.SetTrigger("stop");
                yield return new WaitForSeconds(Random.Range(2f, 5f));
            }
            else if (action == 1) // Walk
            {
                chickenAnimator.SetTrigger("walk");
                Vector3 randomDirection = transform.position + Random.insideUnitSphere * 5f;
                randomDirection.y = transform.position.y; // Keep on the same level
                agent.SetDestination(randomDirection);

                yield return new WaitForSeconds(Random.Range(3f, 6f));
            }
            else if (action == 2) // Rotate
            {
                chickenAnimator.SetTrigger("stop");
                float randomRotation = Random.Range(-90f, 90f);
                transform.Rotate(Vector3.up, randomRotation);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
            else if (action == 3) // Peck
            {
                chickenAnimator.SetTrigger("peck");
                yield return new WaitForSeconds(Random.Range(1f, 2f)); // Allow peck animation to play
            }
        }

        isWandering = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the fox detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, foxDetectionRadius);
    }
}
