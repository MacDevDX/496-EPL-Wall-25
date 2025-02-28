using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class NewEggSpawner : MonoBehaviour
{
    public GameObject spawnEgg;
    public float timetoSpawn = 10f; // Default time between spawns
    private float spawnCountdown;
    private Animator chickenAnimator;
    private NewChickenAI chickenAI;
    private NavMeshAgent navMeshAgent;
    private bool isLayingEgg = false;
    public FoxDirector FoxDir;
    public ShopManager shopManager;

    void Start()
    {
        spawnCountdown = timetoSpawn;
        chickenAnimator = GetComponent<Animator>();
        chickenAI = GetComponent<NewChickenAI>();
        navMeshAgent = chickenAI.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent
        //shopManager = Object.FindFirstObjectByType<ShopManager>();
    }

    void Update()
    {
        if (chickenAI == null || shopManager == null || navMeshAgent == null) return; // Prevent null reference errors

        spawnCountdown -= Time.deltaTime;

        // Check for inventory-based spawn rate upgrades
        if (shopManager.Inventory != null && shopManager.Inventory.Length > 3)
        {
            int upgradeLevel = shopManager.Inventory[3, 7];
            timetoSpawn = Mathf.Clamp(10f - upgradeLevel, 5f, 10f);
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

        isLayingEgg = false;
        //navMeshAgent.isStopped = false; // Re-enable movement
        chickenAI.StopMovement(false); // Resume movement

    }
}
