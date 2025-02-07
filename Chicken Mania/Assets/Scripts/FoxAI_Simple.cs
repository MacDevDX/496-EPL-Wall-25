using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoxAI_Simple : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRadius = 10f;
    public float eatTime = 2f;

    private Transform target;
    private bool isEating = false;
    private static HashSet<Transform> targetedObjects = new HashSet<Transform>(); //Shared set of targeted objects

    void Update()
    {
        if (!isEating)
        {
            FindTarget();
            MoveToTarget();
        }
    }

    void FindTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Draggable"); //Find all with tag Draggable
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (GameObject obj in potentialTargets)
        {
            if (targetedObjects.Contains(obj.transform)) continue; //Skip if another fox is chasing this

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance && distance <= detectionRadius)
            {
                closestDistance = distance;
                closestTarget = obj.transform;
            }
        }

        if (closestTarget != null)
        {
            if (target != null) targetedObjects.Remove(target); //Release old target
            target = closestTarget;
            targetedObjects.Add(target); //Claim new target
        }
    }

    void MoveToTarget()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            StartCoroutine(EatTarget());
        }
    }

    IEnumerator EatTarget()
    {
        isEating = true;
        yield return new WaitForSeconds(eatTime);

        if (target != null)
        {
            targetedObjects.Remove(target);
            Destroy(target.gameObject);
        }

        target = null;
        isEating = false;
    }
}
