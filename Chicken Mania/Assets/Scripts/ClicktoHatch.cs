using UnityEngine;
using TouchScript.Gestures;
using System.Collections;

public class ClicktoHatch : MonoBehaviour
{
    public float clicktoHatch, hatchCountdown;
    public GameObject chickObject;
    
    // these public references will be added by parent upon creation, leave empty in inspector
    public ShopManager shopManager;
    public FoxDirector FoxDir;

    private Animator eggAnimator;

    void Awake()
    {
        // Initialize the hatch countdown
        hatchCountdown = clicktoHatch;

        // Access ShopManager for inventory and currency updates
        // parent object will load shopmanager instead
        //shopManager = Object.FindFirstObjectByType<ShopManager>();
        eggAnimator = GetComponent<Animator>();
        // Set up TapGesture component for detecting taps on the object
        TapGesture tapGesture = gameObject.AddComponent<TapGesture>();
        tapGesture.Tapped += OnTouchTap;
    }

    private void OnTouchTap(object sender, System.EventArgs e)
    {
        // When the object is tapped, reduce the countdown
        hatchCountdown -= 1 + shopManager.Inventory[3, 10];
        shopManager.ResetInactivityTimer();
        GetComponent<AnimatedEgg>().RegisterTap();
    }

    // Update is called once per frame
    void Update()
    {
        if (hatchCountdown <= 0)  // If the countdown has finished
        {
            // Spawn the chick and hatch the egg
            Destroy(gameObject);

            GameObject newChick = Instantiate(chickObject, transform.position, transform.rotation);
            newChick.transform.SetParent(transform.parent);
            shopManager.HatchEgg();

            //IMPORTANT: Currently eggs spawn chickens. If this changes and chicks are implemented, change below to "CHICK"
            // Also, when chicks are implemented, change <NewEggSpawner> below to <chickGrowth>
            FoxDir.setupNewEdible(newChick, shopManager, FoxDir, "CHICK");
            newChick.GetComponent<chickGrowth>().FoxDir = FoxDir;
            newChick.GetComponent<chickGrowth>().shopManager = shopManager;
        }
    }

}
