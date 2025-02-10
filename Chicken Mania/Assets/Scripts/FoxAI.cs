using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;
using TouchScript.Gestures;

public class FoxAI : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float eatingDistance = 1f;
    public float eatingTime = 2f;
    public float wanderRadius = 5f;
    public float wanderTime = 3f;

    private NavMeshAgent agent;
    private GameObject target;
    private bool isEating = false;
    private float eatingStartTime;
    private int clickCount = 0;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (isEating)
        {
            if (target == null || Vector3.Distance(transform.position, target.transform.position) > eatingDistance)
            {
                StopEating();
                return;
            }

            if (Time.time - eatingStartTime >= eatingTime)
            {
                Destroy(target);
                StopEating();
            }
            return;
        }

        FindTarget();
        if (target != null)
        {
            agent.SetDestination(target.transform.position);

            // If close enough to the target, start eating
            if (Vector3.Distance(transform.position, target.transform.position) <= eatingDistance)
            {
                StartEating();
            }
        }
    }

    void FindTarget()
    {
        if (target != null) return;
        GameObject[] chickens = GameObject.FindGameObjectsWithTag("Draggable");
        chickens = chickens.Where(ch => !IsTargetedByOtherFoxes(ch)).ToArray();

        if (chickens.Length > 0)
        {
            target = chickens.OrderBy(ch => Vector3.Distance(transform.position, ch.transform.position)).FirstOrDefault();
        }
    }

    bool IsTargetedByOtherFoxes(GameObject chicken)
    {
        FoxAI[] foxes = Object.FindObjectsByType<FoxAI>(FindObjectsSortMode.None);
        return foxes.Any(fox => fox.target == chicken && fox != this);
    }

    void StartEating()
    {
        if (isEating || target == null) return;
        isEating = true;
        eatingStartTime = Time.time;
        agent.isStopped = true; //Stop the fox while eating
    }

    void StopEating()
    {
        if (target != null)
        {
            UpdateCount(target); // Update the respective count before destroying the target
            Destroy(target);
        }
        isEating = false;
        target = null;
        agent.isStopped = false;
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (target == null)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                agent.SetDestination(hit.position);
            }
            yield return new WaitForSeconds(wanderTime);
        }
    }

    void UpdateCount(GameObject obj)
    {
        // Check the tag of the object and update the corresponding count
        if (obj.name.Contains("egg"))
        {
            shopManager.LoseEgg();
        }
        else if (obj.name.Contains("chicks"))
        {
            shopManager.LoseChick();
        }
        else if (obj.name.Contains("chicken"))
        {
            shopManager.LoseChicken();
        }
    }

    void OnMouseDown()
    {
        RegisterClick();
    }

    public void RegisterTouch() // Call this method from TouchScript events
    {
        RegisterClick();
    }

    private void RegisterClick()
    {
        clickCount++;
        if (clickCount >= 3)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPinch() // Call this method from a pinch gesture in TouchScript
    {
        Destroy(gameObject);
    }
}
