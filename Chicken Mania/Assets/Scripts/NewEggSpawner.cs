using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class NewEggSpawner : MonoBehaviour
{
    public GameObject spawnEgg;
    public GameObject goldenEgg;
    public float timetoSpawn = 10f; // Default time between spawns
    private float spawnCountdown;
    private Animator chickenAnimator;
    private NewChickenAI chickenAI;
    private NavMeshAgent navMeshAgent;
    private bool isLayingEgg = false;
    public FoxDirector FoxDir;
    public ShopManager shopManager;

    private bool menuIsOpen = false;

    void Start()
    {
        //spawnCountdown = timetoSpawn;
        spawnCountdown = shopManager.timeToSpawn;
        chickenAnimator = GetComponent<Animator>();
        chickenAI = GetComponent<NewChickenAI>();
        navMeshAgent = chickenAI.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent
        //shopManager = Object.FindFirstObjectByType<ShopManager>();
        shopManager.MenuOpen += HandleMenuOpen;
    }

    void Update()
    {
        if (chickenAI == null || shopManager == null || navMeshAgent == null) return; // Prevent null reference errors

        if (!menuIsOpen)
        {
            spawnCountdown -= Time.deltaTime;
        }
        

        // Check for inventory-based spawn rate upgrades
        if (shopManager.Inventory != null && shopManager.Inventory.Length > 3)
        {
            int upgradeLevel = shopManager.Inventory[3, 8];
            timetoSpawn = Mathf.Clamp(shopManager.timeToSpawn - upgradeLevel, 3f, 30f); //3f and 30f is min and max values
            timetoSpawn = Mathf.Clamp(shopManager.timeToSpawn * (1 - 0.05f * upgradeLevel), 3f, 30f);
            spawnCountdown = Mathf.Min(spawnCountdown, timetoSpawn); // Adjust spawn countdown
        }
        if (isLayingEgg)
        {
            chickenAnimator.SetTrigger("stop");
        }
        if (spawnCountdown <= 0 && chickenAI.IsStationary() && !isLayingEgg && Random.Range(0, 2) == 0)
        {
            isLayingEgg = true;
            //navMeshAgent.isStopped = true; // Disable movement
            chickenAI.StopMovement(true); // Stop chicken movement
            chickenAnimator.SetTrigger("lay");
            spawnCountdown = timetoSpawn;
        }
    }

    public bool IsLaying()
    {
        return isLayingEgg;
    }
    /*
    public void LayEgg()
    {
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f;
        GameObject newEgg = Instantiate(spawnEgg, eggSpawnPosition, Quaternion.identity);

        // Change parent to screen
        newEgg.transform.SetParent(transform.parent);

        shopManager.AddEgg();

        // this will give this object's shopmanage reference to the new egg and allow the foxes to target it
        FoxDir.setupNewEdible(newEgg, shopManager, FoxDir, "EGG");
        newEgg.GetComponent<ClicktoHatch>().FoxDir = FoxDir;
        newEgg.GetComponent<ClicktoHatch>().shopManager = shopManager;

        // register this egg for decaying
        shopManager.EggDecay.edibleList.Add(newEgg.GetComponent<Edible>());

        isLayingEgg = false;
        //navMeshAgent.isStopped = false; // Re-enable movement
        chickenAI.StopMovement(false); // Resume movement

    }
    */
    public void LayEgg()
    {
        Vector3 eggSpawnPosition = transform.position - transform.forward * 0.5f;

        //Random chance to spawn a golden egg (1 in 500)
        bool isGoldenEgg = Random.Range(1, shopManager.GoldEggChance) == 1;
        Debug.Log($"{shopManager.GoldEggChance}");

        GameObject newEgg;

        if (isGoldenEgg)
        {
            newEgg = Instantiate(goldenEgg, eggSpawnPosition, Quaternion.identity);

            FoxDir.setupNewEdible(newEgg, shopManager, FoxDir, "EGG");
            newEgg.GetComponent<ClickforGold>().FoxDir = FoxDir;
            newEgg.GetComponent<ClickforGold>().shopManager = shopManager;

            shopManager.EggDecay.edibleList.Add(newEgg.GetComponent<Edible>());
        }
        else
        {
            newEgg = Instantiate(spawnEgg, eggSpawnPosition, Quaternion.identity);

            FoxDir.setupNewEdible(newEgg, shopManager, FoxDir, "EGG");
            newEgg.GetComponent<ClicktoHatch>().FoxDir = FoxDir;
            newEgg.GetComponent<ClicktoHatch>().shopManager = shopManager;

            shopManager.EggDecay.edibleList.Add(newEgg.GetComponent<Edible>());
        }

        newEgg.transform.SetParent(transform.parent);

        shopManager.AddEgg();

        isLayingEgg = false;
        chickenAI.StopMovement(false);
    }


    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }
}
