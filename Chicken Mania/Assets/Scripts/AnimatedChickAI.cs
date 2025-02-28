using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChickAI1 : MonoBehaviour
{
    public float movementSpeed = 60f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool isPecking = false;
    private bool isStationary = true; // Default state is stationary
    private Animator chickAnimator;


    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chickAnimator = GetComponent<Animator>();
        // Make chicken generate in random direction
        float randomY = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(0, randomY, 0);
    }

    // Update is called once per frame
    void Update()
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
            chickAnimator.SetTrigger("walk");
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (isRotatingLeft == true)
        {
            chickAnimator.SetTrigger("walk");
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        if (isWalking == true)
        {
            rb.AddForce(transform.forward * movementSpeed);
            chickAnimator.SetTrigger("walk");
        }
        if (isRotatingLeft == false || isRotatingRight == false)
        {
            chickAnimator.SetTrigger("stop");
        }
        if (isWandering == true)
        {
            chickAnimator.SetTrigger("stop");
        }
        if (isPecking == true)
        {
            chickAnimator.SetTrigger("peck");
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
}