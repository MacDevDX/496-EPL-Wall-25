using UnityEngine;

public class chickGrowth : MonoBehaviour
{
    public GameObject spawnChicken;

    public float timetoGrow, growCountdown;

    public ShopManager shopManager;
    public FoxDirector FoxDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        growCountdown = timetoGrow;
    }

    private void Awake()
    {
        // dont use this, it pools all 3 game instances into one shopmanager
        // instead, the object creating this object will load in the shopmanager reference
        //shopManager = Object.FindFirstObjectByType<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        growCountdown -= Time.deltaTime;

        if (growCountdown <= 0)
        {
            growCountdown = timetoGrow;
            Destroy(gameObject);
            GameObject newChicken = Instantiate(spawnChicken, transform.position, transform.rotation);
            newChicken.transform.SetParent(transform.parent);

            shopManager.ChickGrowsToChicken();

            FoxDir.setupNewEdible(newChicken, shopManager, FoxDir, "CHICKEN");
            newChicken.GetComponent<NewEggSpawner>().FoxDir = FoxDir;
            newChicken.GetComponent<NewEggSpawner>().shopManager = shopManager;
        }
    }
}
