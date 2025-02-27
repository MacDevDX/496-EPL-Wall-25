using UnityEngine;

public class EggSpawner : MonoBehaviour
{

    public GameObject spawnEgg;

    public float timetoSpawn, spawnCountdown;

    public ShopManager shopManager;
    public FoxDirector FoxDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnCountdown = timetoSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCountdown -= Time.deltaTime;

        if (spawnCountdown <= 0)
        {
            spawnCountdown = timetoSpawn;

            GameObject newEgg = Instantiate(spawnEgg, transform.position, transform.rotation);
            shopManager.AddEgg();

            FoxDir.setupNewEdible(newEgg, shopManager, FoxDir, "EGG");
            newEgg.GetComponent<ClicktoHatch>().FoxDir = FoxDir;
            newEgg.GetComponent<ClicktoHatch>().shopManager = shopManager;
        }
    }
    private void Awake()
    {
        // parent will load this for us
        //shopManager = Object.FindFirstObjectByType<ShopManager>();
    }

}
