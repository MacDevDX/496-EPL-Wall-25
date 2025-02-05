using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEggSpawner : MonoBehaviour
{

    public GameObject spawnEgg;
    public float timetoSpawn = 5f; // Time between spawns
    public float spawnCountdown;
    private Animator chickenAnimator;
    private AnimatedChickenAI1 animatedChickenAI;
    private bool isLayingEgg = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnCountdown = timetoSpawn;
        chickenAnimator = GetComponent<Animator>();
        animatedChickenAI = GetComponent<AnimatedChickenAI1>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCountdown -= Time.deltaTime;
        if (isLayingEgg)
        {
            chickenAnimator.SetTrigger("stop");
        }

        if (spawnCountdown <= 0 && animatedChickenAI.IsStationary()) 
        {
            isLayingEgg = true;
            chickenAnimator.SetTrigger("lay");
            spawnCountdown = timetoSpawn;
        }
    }
    public bool IsLaying()
    {
        return isLayingEgg;
    }

    public void LayEgg()
    {
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f; // Spawn position slightly behind the chicken
        Instantiate(spawnEgg, eggSpawnPosition, transform.rotation);

        //IgnoreDraggableCollisions(Instantiate(spawnEgg, eggSpawnPosition, transform.rotation)); //Used to ignore collision

        isLayingEgg = false;
    }

    //Vector3 offsetBehind = new Vector3(0, 0, -1); // Behind the chicken (along the Z-axis)
    //Vector3 spawnPosition = transform.position - transform.forward * 1f; // 1f is the distance behind the chicken
    //spawnPosition.y = transform.position.y - 0.5f; // Adjust 0.5f to set the height
    //        Instantiate(spawnEgg, spawnPosition, transform.rotation);


    /*Below is to ignore collision with tag Draggable*/
    private void IgnoreDraggableCollisions(GameObject newObject)
    {
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject obj in draggableObjects)
        {
            if (obj != newObject)
            {
                Collider objCollider = obj.GetComponent<Collider>();
                Collider newCollider = newObject.GetComponent<Collider>();

                if (objCollider != null && newCollider != null)
                {
                    Physics.IgnoreCollision(objCollider, newCollider);
                }
            }
        }
    }
}
