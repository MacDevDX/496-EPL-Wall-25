using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class FoxAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float eatTime = 2f;
    public float wanderRadius = 5f;
    public float idleTime = 3f;
    public float wanderCooldown = 5f;

    private Transform target;
    private bool isEating = false;
    private bool isWandering = false;
    private bool isIdle = false;

    private NavMeshAgent agent;
    private static HashSet<Transform> targetedObjects = new HashSet<Transform>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        if (isEating || isIdle) return;

        FindTarget();

        if (target != null)
        {
            MoveToTarget();
        }
    }

    void FindTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Draggable");
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject obj in potentialTargets)
        {
            if (targetedObjects.Contains(obj.transform)) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)
            {
                closestDistance = distance;
                closestTarget = obj.transform;
            }
        }

        if (closestTarget != null)
        {
            if (target != null) targetedObjects.Remove(target);
            target = closestTarget;
            targetedObjects.Add(target);
            isWandering = false;
        }
    }

    void MoveToTarget()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            StartCoroutine(EatTarget());
        }
    }

    IEnumerator EatTarget()
    {
        isEating = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(eatTime);

        if (target != null)
        {
            targetedObjects.Remove(target);
            Destroy(target.gameObject);
        }

        target = null;
        agent.isStopped = false;
        isEating = false;
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (target == null && !isEating)
            {
                if (!isWandering)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
                    {
                        agent.SetDestination(hit.position);
                        isWandering = true;
                    }
                }
                else if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    isWandering = false;
                    isIdle = true;
                    yield return new WaitForSeconds(idleTime);
                    isIdle = false;
                }
            }
            yield return new WaitForSeconds(wanderCooldown);
        }
    }
}
