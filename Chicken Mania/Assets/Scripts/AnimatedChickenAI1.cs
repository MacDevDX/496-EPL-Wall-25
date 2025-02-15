using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;

public class AnimatedChickenAI1 : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;
    //public bool sellMode = false;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool isPecking = false;
    private bool isStationary = true; // Default state is stationary
    private bool isLayingEggInEggSpawnerScript = false; // Flag to freeze movement while laying anim is playing
    private Animator chickenAnimator;
    private AnimatedEggSpawner animatedEggSpawner;
    private Rigidbody rb;
    private TransformGesture dragGesture;
    private Vector3 velocity = Vector3.zero;
    private bool isDragging = false;
    private ShopManager shopManager;
    private GameObject currentDropZone = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chickenAnimator = GetComponent<Animator>();
        animatedEggSpawner = GetComponent <AnimatedEggSpawner>();
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;  // Prevent unwanted movement
        }

        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;
        dragGesture.TransformCompleted += OnDragEnd; // Detect drag release

        //StartCoroutine(Wander());
    }
    void Update()
    {
        isLayingEggInEggSpawnerScript = animatedEggSpawner.IsLaying();
        if (isLayingEggInEggSpawnerScript)
        {
            isDragging = true;
            return; // Stop movement if laying or being dragged
        }
        else
        {
            isDragging = false; // Allow movement again
        }

        if (!isDragging) // Normal AI movement
        {
            if (isWandering == false)
            {
                StartCoroutine(Wander());
            }
            if (isRotatingRight || isRotatingLeft || isWalking)
            {
                isStationary = false;
            }
            else
            {
                isStationary = true;
            }
            if (isRotatingRight == true)
            {
                chickenAnimator.SetTrigger("walk");
                transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
            }
            if (isRotatingLeft == true)
            {
                chickenAnimator.SetTrigger("walk");
                transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
            }
            if (isWalking == true)
            {
                rb.AddForce(transform.forward * movementSpeed);
                chickenAnimator.SetTrigger("walk");
            }
            if (isRotatingLeft == false || isRotatingRight == false)
            {
                chickenAnimator.SetTrigger("stop");
            }
            if (isWandering == true)
            {
                chickenAnimator.SetTrigger("stop");
            }
            if (isPecking == true)
            {
                chickenAnimator.SetTrigger("peck");
            }
        }
        //rb.WakeUp();
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotationWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(3, 5);
        int peckTime = Random.Range(1, 3);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotationWait);

        if (rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
            isPecking = true;
            yield return new WaitForSeconds(peckTime);
            isPecking = false;
        }
        if (rotateDirection == 2)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
            isPecking = true;
            yield return new WaitForSeconds(peckTime);
            isPecking = false;
        }

        isWandering = false;
    }

    private void OnEnable()
    {
        dragGesture.Transformed += OnDrag;
        dragGesture.TransformCompleted += OnDragEnd;
    }

    private void OnDisable()
    {
        dragGesture.Transformed -= OnDrag;
        dragGesture.TransformCompleted -= OnDragEnd;
    }

    private void OnDrag(object sender, System.EventArgs e)
    {
        if (shopManager.dragZone && shopManager.dragZone.activeSelf)
        {
            isDragging = true;
            rb.useGravity = false;
            rb.isKinematic = true; // Prevents physics interference

            // Convert touch position to world position
            Vector3 screenPosition = dragGesture.ScreenPosition;
            screenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain depth

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // Ensure movement only on X/Z axis
            Vector3 newPosition = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);

            // Smoothly move using Rigidbody to keep collisions
            rb.MovePosition(Vector3.Lerp(transform.position, newPosition, 0.3f));
        }
    }

    private void OnDragEnd(object sender, System.EventArgs e)
    {
        isDragging = false;
        rb.useGravity = true;
        rb.isKinematic = false; // Re-enable physics
    }

    private void SellChicken()
    {
        Sell sellScript = currentDropZone.GetComponent<Sell>();
        if (sellScript != null)
        {
            sellScript.GiveMoney(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            currentDropZone = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            currentDropZone = null;
        }
    }
    public bool IsStationary()
    {
        return isStationary;
    }
}