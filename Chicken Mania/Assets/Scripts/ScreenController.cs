using UnityEngine;
using TouchScript.Gestures;

public class ScreenController : MonoBehaviour
{
    public GameObject startUI;  // Assign Canvas StartUI in the Inspector
    public GameObject gameUI;   // Assign Canvas GameUI in the Inspector
    public GameObject gameObjects; // Assign GameObjects in the Inspector
    public GameObject shopManager; // Assign ShopManager in the Inspector
    public string screenName;   // For debugging purposes

    [Header("Game Start Buttons")]
    public GameObject GameUI_TimerMode;
    public GameObject ManiaModeButton; //Assign button for Mania Mode in the Inspector
    public GameObject TimerModeButton; //Assign button for Timed Mode in the Inspector
    public GameObject TycoonModeButton; //Assign button for Tycoon Mode in the Inspector

    [Header("Platform")]
    public GameObject NormalGround; //Assign the normal polyobject for normal mode
    public GameObject TimedGround;  //Assign the timed polyobject for timed mode

    private TapGesture tapGesture; // TouchScript's Tap Gesture

    public ShopManager shopManagerScript; // Reference to ShopManager script
    public InactivityHandler InactivityScript; // Reference to InactivityHandler script

    private bool gameModeStarted = false;

    public float timetoGrow = 10f;
    public float timetoSpawn = 10f;

    private void OnEnable()
    {
        // Get the TapGesture component and subscribe to the event
        tapGesture = GetComponent<TapGesture>();
        if (tapGesture != null)
        {
            tapGesture.Tapped += OnTap;
            tapGesture.StateChanged += OnGestureStateChanged; // Listen to state changes
        }
    }

    // Handle the gesture state change(for more advanced gesture tracking)
    private void OnGestureStateChanged(object sender, GestureStateChangeEventArgs e)
    {
        // For debugging, check the gesture's state
        //Debug.Log($"Gesture state changed from {e.PreviousState} to {e.State} on {screenName}");
    }

    private void OnDisable()
    {
        // Unsubscribe from eventsks
        if (tapGesture != null)
        {
            tapGesture.Tapped -= OnTap;
            tapGesture.StateChanged -= OnGestureStateChanged;
        }
    }

    private void OnTap(object sender, System.EventArgs e)
    {
        GameObject tappedObject = ((Component)sender).gameObject;

        if (tappedObject == ManiaModeButton)
        {
            StartGame();
        }
        else if (tappedObject == TimerModeButton)
        {
            TGameMode();
        }
        else if (tappedObject == TycoonModeButton)
        {
            StartTycoonGameMode();
        }
    }

    public void StartGame()
    {
        startUI.SetActive(false);  // Hide start UI
        gameUI.SetActive(true);    // Show in-game UI

        CanvasGroup canvasGroup = gameUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameUI.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 255;  // Make it visible
        canvasGroup.interactable = true;  // enable interaction
        canvasGroup.blocksRaycasts = true; // blocks clicks

        gameObjects.SetActive(true); // Activate game objects
        NormalGround.SetActive(true);
        TimedGround.SetActive(false);
        shopManager.SetActive(true); // Activate shop manager

        InactivityScript.inactivityThreshold = 60f; //Time set to higher than the game's time mode
        shopManagerScript.timeToGrow = 10f;
        shopManagerScript.timeToSpawn = 10f;
    }

    public void StartTycoonGameMode()
    {
        startUI.SetActive(false);  // Hide start UI
        gameUI.SetActive(true);    // Show in-game UI

        CanvasGroup canvasGroup = gameUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameUI.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 255;  // Make it visible
        canvasGroup.interactable = true;  // enable interaction
        canvasGroup.blocksRaycasts = true; // blocks clicks

        gameObjects.SetActive(true); // Activate game objects
        NormalGround.SetActive(true);
        TimedGround.SetActive(false);
        shopManager.SetActive(true); // Activate shop manager

        InactivityScript.inactivityThreshold = 60f; //Time set to higher than the game's time mode
        shopManagerScript.timeToGrow = 50f;
        shopManagerScript.timeToSpawn = 30f;
        shopManagerScript.GoldEggChance = 1000000;
    }

    public void ReturnToTitlePage()
    {
        GameUI_TimerMode.SetActive(false);
        gameUI.SetActive(false);
        gameObjects.SetActive(false);
        shopManager.SetActive(false);
        startUI.SetActive(true);
        gameModeStarted = false;
    }

    public void TGameMode()
    {
        if (!gameModeStarted)
        {
            startUI.SetActive(false);  // Hide start UI
            gameObjects.SetActive(true); // Activate game objects
            NormalGround.SetActive(false);
            TimedGround.SetActive(true);
            shopManager.SetActive(true); // Activate shop manager
            gameUI.SetActive(true);
            CanvasGroup canvasGroup = gameUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;  // Make it fully transparent
            canvasGroup.interactable = false;  // Disable interaction
            canvasGroup.blocksRaycasts = false; // Prevents blocking clicks

            GameUI_TimerMode.SetActive(true);
            shopManagerScript.StartCountdown();
            //shopManagerScript.Inventory[3, 9] = 2;  //DOESN'T SET INITIALLY
            shopManagerScript.timeToGrow = 7f;
            shopManagerScript.timeToSpawn = 7f;
            shopManagerScript.GoldEggChance = 1000000;

            InactivityScript.inactivityThreshold = 600f; //Time set to higher than the game's time mode
            gameModeStarted = true;
        }
        /*
        // If somehow they lose all chickens/chicks/eggs then.. spawn 1 so doesn't trigger the normal Gamemode's gameover
        // Probably not needed because if money=20, they will stay until timer reaches 0.. for proper exit
        if (shopManagerScript.chickensCount + shopManagerScript.chicksCount + shopManagerScript.eggsCount <= 0)
        {
            int itemId = Random.Range(1, 7); // Randomly select an item ID between 1 and 6
            if (itemId >= 1 && itemId <= 6)
            {
                shopManagerScript.SpawnChicken(itemId);
                shopManagerScript.AddChicken();
            }
        }
        */

    }
    public void ProtectGameMode()
    {
        if (!gameModeStarted)
        {
            startUI.SetActive(false);  // Hide start UI
            gameObjects.SetActive(true); // Activate game objects
            NormalGround.SetActive(false);
            TimedGround.SetActive(true);
            shopManager.SetActive(true); // Activate shop manager
            gameUI.SetActive(true);
            CanvasGroup canvasGroup = gameUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0;  // Make it fully transparent
            canvasGroup.interactable = false;  // Disable interaction
            canvasGroup.blocksRaycasts = false; // Prevents blocking clicks

            GameUI_TimerMode.SetActive(true);
            shopManagerScript.StartCountdownPGM();

            shopManagerScript.timeToSpawn = 600f;


            InactivityScript.inactivityThreshold = 600f; //Time set to higher than the game's time mode
            gameModeStarted = true;
        }


    }
}
