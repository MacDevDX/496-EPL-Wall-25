using System.Collections;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using TouchScript.Gestures.TransformGestures;

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
    private TransformGesture dragGesture;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        agent = GetComponent<NavMeshAgent>();
        chickenAnimator = GetComponent<Animator>();
        agent.speed = movementSpeed;

        // Add Transform Gesture
        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;


        eggTimer = eggLayInterval; // Initialize egg timer
        StartCoroutine(Wander());
    }

    void Update()
    {
        // Handle egg-laying timer
        if (!isLayingEgg && !isRunningFromFox)
        {
            eggTimer -= Time.deltaTime;
            if (eggTimer <= 0f)
            {
                StartCoroutine(LayEgg());
                eggTimer = eggLayInterval; // Reset timer
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
            StartCoroutine(Wander()); // Resume wandering
        }

        // Synchronize animations with NavMeshAgent
        if (!isLayingEgg && !isRunningFromFox)
        {
            if (agent.velocity.magnitude > 0.1f) // Agent is moving
            {
                chickenAnimator.SetTrigger("walk");
            }
            else if (agent.remainingDistance <= agent.stoppingDistance) // Agent has stopped
            {
                chickenAnimator.SetTrigger("stop");
            }
        }
    }

    IEnumerator LayEgg()
    {
        isLayingEgg = true;
        agent.isStopped = true; // Stop movement while laying an egg
        chickenAnimator.SetTrigger("lay"); // Trigger "lay" animation

        //yield return new WaitForSeconds(2f); // Wait for the duration of the "lay" animation

        // Spawn the egg
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f;
        Instantiate(eggPrefab, eggSpawnPosition, Quaternion.identity);

        shopManager.AddEgg();
        chickenAnimator.SetTrigger("stop");

        isLayingEgg = false;
        agent.isStopped = false; // Resume movement
        yield return null;
    }

    void RunAway(Vector3 foxPosition)
    {
        isRunningFromFox = true;
        agent.isStopped = false; // Ensure agent is moving
        StopCoroutine(Wander()); // Stop wandering

        Vector3 runDirection = (transform.position - foxPosition).normalized * 10f;
        Vector3 targetPosition = transform.position + runDirection;

        agent.SetDestination(targetPosition);
    }

    IEnumerator Wander()
    {
        while (!isLayingEgg && !isRunningFromFox)
        {
            // Choose a random action: walk, idle, or peck
            int action = Random.Range(0, 3); // 0 = walk, 1 = idle, 2 = peck

            if (action == 0) // Walk
            {
                chickenAnimator.SetTrigger("walk");
                Vector3 randomDirection = transform.position + Random.insideUnitSphere * 5f;
                randomDirection.y = transform.position.y; // Keep on the same level
                agent.SetDestination(randomDirection);

                yield return new WaitForSeconds(Random.Range(3f, 6f));
            }
            else if (action == 1) // Idle
            {
                chickenAnimator.SetTrigger("stop");
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
            else if (action == 2) // Peck
            {
                chickenAnimator.SetTrigger("peck");
                yield return new WaitForSeconds(Random.Range(1f, 2f)); // Allow peck animation to play
            }
        }
    }
    private void OnDrag(object sender, System.EventArgs e)
    {
        if (shopManager.dragZone && shopManager.dragZone.activeSelf)
        {
            transform.position += dragGesture.DeltaPosition;
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the fox detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, foxDetectionRadius);
    }
}
