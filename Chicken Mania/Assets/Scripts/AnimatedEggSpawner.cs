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
        Debug.Log("Egg Start called: spawnCountdown set to " + spawnCountdown);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Egg Update called: spawnCountdown = " + spawnCountdown);

        spawnCountdown -= Time.deltaTime;
        if (isLayingEgg)
        {
            Debug.Log("Egg is laying, stopping chicken animation.");
            chickenAnimator.SetTrigger("stop");
        }

        if (spawnCountdown <= 0 && animatedChickenAI.IsStationary()) 
        {
            Debug.Log("Egg countdown finished and chicken is stationary, starting lay animation.");
            isLayingEgg = true;
            chickenAnimator.SetTrigger("lay");
            spawnCountdown = timetoSpawn;
            Debug.Log("Egg countdown reset to " + spawnCountdown);

        }
    }
    public bool IsLaying()
    {
        Debug.Log("Egg laying status checked: " + isLayingEgg);
        return isLayingEgg;
    }

    public void LayEgg()
    {
        Debug.Log("LayEgg: Egg is being laid.");
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f; // Spawn position slightly behind the chicken
        Instantiate(spawnEgg, eggSpawnPosition, transform.rotation);
        isLayingEgg = false;
        Debug.Log("LayEgg: Egg laid, isLayingEgg set to false.");

    }

    //Vector3 offsetBehind = new Vector3(0, 0, -1); // Behind the chicken (along the Z-axis)
    //Vector3 spawnPosition = transform.position - transform.forward * 1f; // 1f is the distance behind the chicken
    //spawnPosition.y = transform.position.y - 0.5f; // Adjust 0.5f to set the height
    //        Instantiate(spawnEgg, spawnPosition, transform.rotation);
}
