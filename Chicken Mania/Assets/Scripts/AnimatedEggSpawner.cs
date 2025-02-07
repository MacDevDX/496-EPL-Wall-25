using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEggSpawner : MonoBehaviour
{

    public GameObject spawnEgg;
    public float timetoSpawn = 10f; // Time between spawns
    public float spawnCountdown;
    private Animator chickenAnimator;
    private AnimatedChickenAI1 animatedChickenAI;
    private bool isLayingEgg = false;

    private ShopManager shopManager;


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

        if (shopManager.Inventory[3, 7] == 1)
        {
            timetoSpawn = 7f;
        }
        else if (shopManager.Inventory[3, 7] == 2)
        {
            timetoSpawn = 6f;
        }
        else if (shopManager.Inventory[3, 7] == 3)
        {
            timetoSpawn = 5f;
        }

        if (isLayingEgg)
        {
            chickenAnimator.SetTrigger("stop");
        }

        if (spawnCountdown <= 0 && animatedChickenAI.IsStationary() && Random.Range(0, 2) == 0) //50% to lay egg
        {
            isLayingEgg = true;
            chickenAnimator.SetTrigger("lay");
            spawnCountdown = timetoSpawn;
        }
    }
    private void Awake()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();
    }
    public bool IsLaying()
    {
        return isLayingEgg;
    }

    public void LayEgg()
    {
        Vector3 eggSpawnPosition = transform.position *0.3f - transform.forward * 0.5f; // Spawn position slightly behind the chicken
        Instantiate(spawnEgg, eggSpawnPosition, transform.rotation);
        shopManager.AddEgg();
        isLayingEgg = false;
    }

    //Vector3 offsetBehind = new Vector3(0, 0, -1); // Behind the chicken (along the Z-axis)
    //Vector3 spawnPosition = transform.position - transform.forward * 1f; // 1f is the distance behind the chicken
    //spawnPosition.y = transform.position.y - 0.5f; // Adjust 0.5f to set the height
    //        Instantiate(spawnEgg, spawnPosition, transform.rotation);
}
