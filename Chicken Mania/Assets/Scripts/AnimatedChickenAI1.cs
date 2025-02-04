using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedChickenAI1 : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool isPecking = false;
    private bool isStationary = true; // Default state is stationary
    private bool isLayingEggInEggSpawnerScript = false; // Flag to freeze movement while laying anim is playing
    private Animator chickenAnimator;
    private AnimatedEggSpawner animatedEggSpawner;


    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chickenAnimator = GetComponent<Animator>();
        animatedEggSpawner = GetComponent <AnimatedEggSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        isLayingEggInEggSpawnerScript = animatedEggSpawner.IsLaying();
        if (isLayingEggInEggSpawnerScript)
        {
            return; // Stop movement if laying
        }

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

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotationWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 2);
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
    // Use case: ensures that chicken is not moving when laying an egg
    public bool IsStationary()
    {
        return isStationary;
    }

    //public void SetLayingFlag(bool laying)
    //{
    //    isLayingEggInEggSpawnerScript = laying;
        
    //    if (laying)
    //    {
    //        rb.linearVelocity = Vector3.zero; // Stop movement
    //        rb.angularVelocity = Vector3.zero;
    //        chickenAnimator.SetTrigger("stop"); // Stop all animations
    //    }
    //}
}