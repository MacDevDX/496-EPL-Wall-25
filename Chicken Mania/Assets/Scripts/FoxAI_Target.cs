using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxAI_Target : MonoBehaviour
{
    public float detectionRange = 10f;
    public float eatDistance = 1f; // Fox must be within this distance to eat
    public float eatTime = 2f; // Time to consume target
    public float wanderRadius = 5f;
    public float wanderTime = 3f;

    private NavMeshAgent agent;
    private GameObject target;
    private bool isEating = false;
    private static HashSet<GameObject> targetedChickens = new HashSet<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (isEating) return;

        if (target == null || !target.activeSelf)
        {
            FindTarget();
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= eatDistance)
            {
                StartCoroutine(EatTarget());
            }
            else
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }

    void FindTarget()
    {
        GameObject[] chickens = GameObject.FindGameObjectsWithTag("Draggable");
        float closestDistance = detectionRange;
        GameObject closestChicken = null;

        foreach (GameObject chicken in chickens)
        {
            if (!targetedChickens.Contains(chicken))
            {
                float distance = Vector3.Distance(transform.position, chicken.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestChicken = chicken;
                }
            }
        }

        if (closestChicken != null)
        {
            target = closestChicken;
            targetedChickens.Add(target);
            agent.SetDestination(target.transform.position);
        }
    }

    IEnumerator EatTarget()
    {
        isEating = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(eatTime);

        if (target != null)
        {
            targetedChickens.Remove(target);
            Destroy(target);
        }

        target = null;
        agent.isStopped = false;
        isEating = false;
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (target == null)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
                {
                    agent.SetDestination(hit.position);
                }
            }
            yield return new WaitForSeconds(wanderTime);
        }
    }
}
