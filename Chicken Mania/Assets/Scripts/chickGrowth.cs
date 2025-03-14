using UnityEngine;

public class chickGrowth : MonoBehaviour
{
    public GameObject spawnChicken;

    public float timetoGrow, growCountdown;

    public ShopManager shopManager;
    public FoxDirector FoxDir;

    private ScreenController ScreenController;
    private bool menuIsOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        growCountdown = shopManager.timeToGrow;
        shopManager.MenuOpen += HandleMenuOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuIsOpen)
        {
            return;
        }

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
            newChicken.GetComponent<NewChickenAI>().shopManager = shopManager;
        }
    }

    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }
}
