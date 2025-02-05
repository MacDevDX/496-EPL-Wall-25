using UnityEngine;

public class EggSpawner : MonoBehaviour
{

    public GameObject spawnEgg;

    public float timetoSpawn, spawnCountdown;

    private ShopManager shopManager;

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

            Instantiate(spawnEgg, transform.position, transform.rotation);
            shopManager.AddEgg();
        }
    }
    private void Awake()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();
    }

}
