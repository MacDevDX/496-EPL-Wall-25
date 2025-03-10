using System.Collections;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;
using UnityEngine.AI;

public class NewChicksAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    public float foxDetectionRadius = 0f;
    public LayerMask foxLayer;

    [Header("Wander Timing Settings")]
    public float minWalkTime = 3f;
    public float maxWalkTime = 6f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;
    public float minPeckTime = 1f;
    public float maxPeckTime = 2f;

    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private TransformGesture dragGesture;
    private bool isDragging = false;
    private bool isRunningFromFox = false;
    private GameObject currentDropZone;

    private ShopManager shopManager;
    private bool menuIsOpen = false;

    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        animator = GetComponent<Animator>();
        shopManager.MenuOpen += HandleMenuOpen;

        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        /*
        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;
        dragGesture.TransformCompleted += (s, e) => OnDragEnd();
        */
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (isDragging) return;

        HandleFoxDetection();
        UpdateAnimationState();
    }

    private void HandleFoxDetection()
    {
        Collider[] foxes = Physics.OverlapSphere(transform.position, foxDetectionRadius, foxLayer);
        if (foxes.Length > 0)
        {
            RunAway(foxes[0].transform.position);
        }
        else if (isRunningFromFox)
        {
            isRunningFromFox = false;
            StartCoroutine(Wander());
        }
    }

    private void UpdateAnimationState()
    {
        if (!isRunningFromFox)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetTrigger("walk");
            }
            else
            {
                animator.SetTrigger("stop");
            }
        }
    }

    private void RunAway(Vector3 foxPosition)
    {
        isRunningFromFox = true;
        StopCoroutine(Wander());

        Vector3 runDirection = (transform.position - foxPosition).normalized * 10f;
        agent.SetDestination(transform.position + runDirection);
    }

    IEnumerator Wander()
    {
        while (!isRunningFromFox)
        {
            int action = Random.Range(0, 3);

            if (menuIsOpen)
            {
                action = 1;
            }

            if (action == 0)
            {
                animator.SetTrigger("walk");
                agent.SetDestination(transform.position + Random.insideUnitSphere * 5f);
                yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));
            }
            else if (action == 1)
            {
                animator.SetTrigger("stop");
                yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
            }
            else
            {
                animator.SetTrigger("peck");
                yield return new WaitForSeconds(Random.Range(minPeckTime, maxPeckTime));
            }
        }
    }
    /*
    private void OnDrag(object sender, System.EventArgs e)
    {
        if (!shopManager.dragZone.activeSelf) return;

        isDragging = true;
        agent.isStopped = true;
        rb.isKinematic = false;

        rb.MovePosition(transform.position + dragGesture.DeltaPosition);
    }

    private void OnDragEnd()
    {
        isDragging = false;
        rb.isKinematic = true;
        agent.Warp(transform.position);
        agent.isStopped = false;

        if (currentDropZone != null)
            SellChick();
    }

    private void SellChick()
    {
        currentDropZone?.GetComponent<Sell>()?.GiveMoney(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
            currentDropZone = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DropZone"))
            currentDropZone = null;
    }
    */
    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }

}
