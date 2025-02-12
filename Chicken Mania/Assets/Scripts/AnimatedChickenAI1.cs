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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chickenAnimator = GetComponent<Animator>();
        animatedEggSpawner = GetComponent <AnimatedEggSpawner>();
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        // Add Transform Gesture
        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;

        StartCoroutine(Wander());
    }
    void Update()
    {
        isLayingEggInEggSpawnerScript = animatedEggSpawner.IsLaying();
        if (isLayingEggInEggSpawnerScript)
        {
            isDragging = true;
            return; // Stop movement if laying or being dragged
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
    private void OnDrag(object sender, System.EventArgs e)
    {
        if (shopManager.dragZone && shopManager.dragZone.activeSelf)
        {
        transform.position += dragGesture.DeltaPosition;
        isDragging = false;
        }
    }

    /*
    private void OnDrag(object sender, System.EventArgs e)
    {
        if (!sellMode) return; // Only allow drag if selling mode is active

        TransformGesture gesture = sender as TransformGesture;
        if (gesture == null) return;

        isDragging = true;

        // Convert screen delta to world position
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);

        // Keep the chicken on the ground (adjust Y position)
        worldPosition.y = transform.position.y;

        // Move smoothly using Rigidbody
        rb.MovePosition(Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * 10));
    }
    */
    public void EnableSellingMode(bool enable)
    {
        //sellMode = enable;
    }

    public bool IsStationary()
    {
        return isStationary;
    }
}