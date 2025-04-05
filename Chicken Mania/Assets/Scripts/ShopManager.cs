using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TouchScript.Examples.RawInput;
using TouchScript.Behaviors;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine.SocialPlatforms.Impl;

public class ShopManager : MonoBehaviour
{
    public int[,] Inventory = new int[4,12]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    //public int startingMoney = 100000;
    public TextMeshProUGUI Money_Text;
    //public TextMeshProUGUI ChickensCount_Text;
    //public TextMeshProUGUI ChicksCount_Text;
    //public TextMeshProUGUI EggsCount_Text;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI tutorialTextPGM;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI ShopMoneyText;
    public TextMeshProUGUI ShopUpgradeText;

    //Chicken Species & Spawn
    public GameObject[] ChickenSpecies;
    public Transform SpawnPoint;

    public int chickensCount = 0;
    public int chicksCount = 0;
    public int eggsCount = 0;

    public GameObject dragZone;
    public GameObject ShopWindow;
    public GameObject UpgradeWindow;
    public FoxDirector FoxDir;
    public EggDecayer EggDecay;

    // Screen Section
    public GameObject screenSection;
    private ScreenController screenController;

    private float Timer = 45f;
    public float timeToGrow = 10f;
    public float timeToSpawn = 10f;
    public float FoxDetection = 0f;
    public int GoldEggChance = 2;
    public GameObject lastSpawnedChicken;
    public GameObject moneyIndicator;
    public float EggValue = 0.25f;
    public float ChickValue = 0.5f;
    public float ChickenValue = 0.6f;
    public bool TycoonMode = false;
    public GameObject HatchStartingMessage;
    public GameObject DefendStartingMessage;

    private TapGesture tapGesture;

    private GameObject activeSellZone;

    // Music and Home Button Menu
    [Header("Settings")]
    public GameObject SettingsButtonMenuMania;
    public GameObject SettingsButtonMenuTycoon;
    public GameObject HomeButtonMenu;
    public GameObject HatchSettingsButtonMenu;
    public GameObject ProtectSettingsButtonMenu;
    public GameObject TimedHomeButtonMenu;

    // Pause Broadcast
    public delegate void MenuOpenEventHandler(object sender, MenuOpenEventArgs e);
    public event MenuOpenEventHandler MenuOpen;

    [Header("Theme Objects")]
    public GameObject ChristmasLights;
    public GameObject Jackolantern;

    [Header("Inactivity Objects")]
    public GameObject inactivityWarningGreen;
    public GameObject inactivityWarningOrange;
    public TextMeshProUGUI greenCountdownText;
    public TextMeshProUGUI orangeCountdownText;
    public GameObject hudObject; // Reference to the HUD object
    public GameObject Screen;
    public float inactivityThreshold = 60f; // Seconds before warning
    public float returnToMenuTime = 30f;    // Seconds before message disappears
    private float lastInteractionTime;
    private float countdownTime;
    private bool countdownStarted = false;

    void Start()
    {
        screenController = screenSection.GetComponent<ScreenController>();
        Money_Text.text = Money.ToString();
        UpdateUI();

        tapGesture = screenController.GetComponent<TapGesture>();

        if (tapGesture != null)
        {
            tapGesture.Tapped += OnUserInteraction;
        }

        /*--------------------------------------------------------------------------
         *************************************************************************** 
         ******************************** Shop ID **********************************
         ***************************************************************************
         --------------------------------------------------------------------------*/

        //Chicken Tiers
        Inventory[1, 1] = 1;
        Inventory[1, 2] = 2;
        Inventory[1, 3] = 3;
        Inventory[1, 4] = 4;
        Inventory[1, 5] = 5;
        Inventory[1, 6] = 6;
        Inventory[1, 7] = 7; //Secret Chicken

        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[1, 8] = 8;
        Inventory[1, 9] = 9;
        Inventory[1, 10] = 10;
        Inventory[1, 11] = 11;


        /*--------------------------------------------------------------------------
         *************************************************************************** 
         ******************************* Pricing ***********************************
         ***************************************************************************
         --------------------------------------------------------------------------*/

        //Chicken Price
        /*
        Inventory[2, 1] = 20;
        Inventory[2, 2] = 30;
        Inventory[2, 3] = 75;
        Inventory[2, 4] = 113;
        Inventory[2, 5] = 281;
        Inventory[2, 6] = 600;
        Inventory[2, 7] = 10000; //Secret Chicken
        
        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[2, 8] = 30;
        Inventory[2, 9] = 25;
        Inventory[2, 10] = 25;
        Inventory[2, 11] = 50;
        */

        /*--------------------------------------------------------------------------
         *************************************************************************** 
         ******************************** Count ************************************
         ***************************************************************************
         --------------------------------------------------------------------------*/

        //Chicken Count
        Inventory[3, 1] = 0;
        Inventory[3, 2] = 0;
        Inventory[3, 3] = 0;
        Inventory[3, 4] = 0;
        Inventory[3, 5] = 0;
        Inventory[3, 6] = 0;
        Inventory[3, 7] = 0;

        //Upgrades Count (Supplements, Feed, Incubator, Research)
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;
        Inventory[3, 11] = 0;

        try
        {
            DateTime currentDate = DateTime.Now;

            if (currentDate != null) // Ensures currentDate is valid
            {
                // Define the start and end of the Christmas week
                DateTime startOfChristmasWeek = new DateTime(currentDate.Year, 12, 18);
                DateTime endOfChristmasWeek = new DateTime(currentDate.Year, 12, 25);

                // Check if the current date falls within the range
                if (currentDate >= startOfChristmasWeek && currentDate <= endOfChristmasWeek)
                {
                    ChristmasLights.SetActive(true);
                }
                // Define the start and end of the Halloween week
                DateTime startOfHalloweenWeek = new DateTime(currentDate.Year, 10, 24);
                DateTime endOfHalloweenWeek = new DateTime(currentDate.Year, 10, 31);

                // Check if the current date falls within Halloween week
                if (currentDate >= startOfHalloweenWeek && currentDate <= endOfHalloweenWeek)
                {
                    Jackolantern.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning("Unable to retrieve the current date.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occurred while checking the date: " + ex.Message);
        }


        lastInteractionTime = Time.time;

        if (inactivityWarningGreen != null)
        {
            inactivityWarningGreen.SetActive(false);
        }
        if (inactivityWarningOrange != null)
        {
            inactivityWarningOrange.SetActive(false);
        }

        RegisterTouchGestures();
    }
    void Update()
    {
        CheckGameOver();

        //Inactivity Handler
        // Check inactivity per instance
        if (!countdownStarted && Time.time - lastInteractionTime > inactivityThreshold)
        {
            ShowInactivityWarning();
        }

        // Handle countdown per instance
        if (countdownStarted)
        {
            countdownTime -= Time.deltaTime;
            string countdownMessage = Mathf.Ceil(countdownTime) + "s";

            if (inactivityWarningGreen != null && inactivityWarningGreen.activeSelf && greenCountdownText != null)
            {
                greenCountdownText.text = countdownMessage;
            }

            if (inactivityWarningOrange != null && inactivityWarningOrange.activeSelf && orangeCountdownText != null)
            {
                orangeCountdownText.text = countdownMessage;
            }

            if (countdownTime <= 0)
            {
                if (inactivityWarningGreen != null)
                {
                    inactivityWarningGreen.SetActive(false);
                }

                if (inactivityWarningOrange != null)
                {
                    inactivityWarningOrange.SetActive(false);
                }

                countdownStarted = false; // Reset so it can trigger again later
                ResetGame();
            }
        }
    }

    protected virtual void OnMenuOpen(MenuOpenEventArgs e)
    {
        MenuOpen?.Invoke(this, e);
    }

    public void Buy()
    {
        //References to the button clicked
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ButtonRef == null)
        {
            return;
        }
        int itemId = ButtonRef.GetComponent<ShopButtons>().ItemID;

        //Check if enough money is available for the purchase
        if (Money >= Inventory[2, itemId])
        {
            //Deduct money and update UI
            Money -= Inventory[2, itemId];
            ShowMoneyIndicator(Inventory[2, itemId], ButtonRef);
            //Money_Text.text = Money.ToString();

            //Increment the count for the item (upgrade)
            Inventory[3, itemId] += 1;

            //If buying a chicken (Index [x,1-6]), spawn it
            if (itemId >= 1 && itemId <= 6)
            {
                SpawnChicken(itemId);
                AddChicken();
                Invoke("RaisePauseEvent", 0.5f);
            }
            if (itemId == 7)
            {
                //SpawnFriedChicken(itemId);
                SpawnChicken(itemId);
                AddChicken();
                //Pause after a short delay after buying chicken
                Invoke("RaisePauseEvent", 0.5f);
            }

            //Only applies multiplier to Upgrade indexes
            if (itemId == 8) //Supplement
            {
                // For Mania Mode
                if ((Inventory[3, itemId] == 1) && !TycoonMode)
                {
                    Inventory[2, itemId] = 60;
                }
                if ((Inventory[3, itemId] == 2) && !TycoonMode)
                {
                    Inventory[2, itemId] = 100;
                }
                // For Tycoon Mode
                if ((Inventory[3, itemId] == 1) && TycoonMode)
                {
                    Inventory[2, itemId] = 75;
                }
                if ((Inventory[3, itemId] == 2) && TycoonMode)
                {
                    Inventory[2, itemId] = 100;
                }
            }
            if (itemId == 9) //Feed
            {
                // For Mania Mode
                if ((Inventory[3, itemId] == 1) && !TycoonMode)
                {
                    Inventory[2, itemId] = 50;
                }
                if ((Inventory[3, itemId] == 2) && !TycoonMode)
                {
                    Inventory[2, itemId] = 100;
                }
                // For Tycoon Mode
                if ((Inventory[3, itemId] == 1) && TycoonMode)
                {
                    Inventory[2, itemId] = 250;
                }
                if ((Inventory[3, itemId] == 2) && TycoonMode)
                {
                    Inventory[2, itemId] = 600;
                }
            }
            if (itemId == 10) //Incubator
            {
                Inventory[2, itemId] = (Inventory[2, itemId] + 25);
            }
            if (itemId == 11) //Research
            {
                // For Mania Mode
                if ((Inventory[3, itemId] == 1) && !TycoonMode)
                {
                    Inventory[2, itemId] = 150;
                }
                if ((Inventory[3, itemId] == 2) && !TycoonMode)
                {
                    Inventory[2, itemId] = 500;
                }
                // For Tycoon Mode
                if ((Inventory[3, itemId] == 1) && TycoonMode)
                {
                    Inventory[2, itemId] = 800;
                }
                if ((Inventory[3, itemId] == 2) && TycoonMode)
                {
                    Inventory[2, itemId] = 3000;
                }
            }
        }
        UpdateUI();
    }
    public void SpawnChicken(int itemId)
    {
        int index = itemId - 1;
        if (index >= 0 && index < ChickenSpecies.Length && ChickenSpecies[index] != null && SpawnPoint != null)
        {
            GameObject newChicken = Instantiate(ChickenSpecies[index], SpawnPoint.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

            // Set chicken as child of screen
            newChicken.transform.SetParent(screenSection.transform);
            lastSpawnedChicken = newChicken;  // Store reference

            NewChickenAI newChickenAI = newChicken.GetComponent<NewChickenAI>(); //Added for drag instances
            newChickenAI.shopManager = this;

            FoxDir.setupNewEdible(newChicken, this, FoxDir, "CHICKEN");

            NewEggSpawner newEggScript = newChicken.GetComponent<NewEggSpawner>();
            newEggScript.FoxDir = FoxDir;
            newEggScript.shopManager = this;

            // Set the egg spawner under the screen
            newEggScript.transform.SetParent(screenSection.transform);
            
        }
    }
    public void SpawnFriedChicken(int itemId)
    {
        int index = itemId - 1;
        if (index >= 0 && index < ChickenSpecies.Length && ChickenSpecies[index] != null && SpawnPoint != null)
        {
            GameObject newChicken = Instantiate(ChickenSpecies[index], SpawnPoint.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

            newChicken.transform.SetParent(screenSection.transform);

            FoxDir.setupNewEdible(newChicken, this, FoxDir, "CHICKEN");
        }
    }

    // call this to pause the game
    private void RaisePauseEvent()
    { 
        OnMenuOpen(new MenuOpenEventArgs(true)); 
    
    }
    /**************** Below is to Toggle the shop menu ****************/

    public void ToggleSell()
    {
        if (activeSellZone == null && !isGameOver)
        {
            activeSellZone = Instantiate(dragZone, SpawnPoint.position, Quaternion.identity);
            Sell sellScript = activeSellZone.GetComponent<Sell>();
            sellScript.shopManager = this;
            activeSellZone.transform.SetParent(screenSection.transform);
        }
        else
        {
            Destroy(activeSellZone);
            activeSellZone = null;
        }
    }

    public void ToggleBuy()
    {
        if (ShopWindow != null && !isGameOver && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (SettingsButtonMenuMania == null || !SettingsButtonMenuMania.activeSelf) && (HomeButtonMenu == null || !HomeButtonMenu.activeSelf) && (SettingsButtonMenuTycoon == null || !SettingsButtonMenuTycoon.activeSelf))
        {
            ShopWindow.SetActive(!ShopWindow.activeSelf); //Toggle the Chicken Shop Window
            OnMenuOpen(new MenuOpenEventArgs(ShopWindow.activeSelf));

            if (ShopWindow.activeSelf == false)
            {
                //Cancel all invokes from shopmanager. This is meant to stop the delayed pause event after buying a chicken. If you add another Invoked method to shopmanager and it breaks, this is why
                CancelInvoke();
            }
        }
    }

    public void ToggleUpgrade()
    {
        if (UpgradeWindow != null && !isGameOver && (ShopWindow == null || !ShopWindow.activeSelf) && (SettingsButtonMenuMania == null || !SettingsButtonMenuMania.activeSelf) && (HomeButtonMenu == null || !HomeButtonMenu.activeSelf) && (SettingsButtonMenuTycoon == null || !SettingsButtonMenuTycoon.activeSelf))
        {
            UpgradeWindow.SetActive(!UpgradeWindow.activeSelf); //Toggle the upgrade shop
            OnMenuOpen(new MenuOpenEventArgs(UpgradeWindow.activeSelf));

            if (UpgradeWindow.activeSelf == false)
            {
                CancelInvoke();
            }
        }
    }
    public void ManiaOpenSettingsButtonMenu()
    {
        if (SettingsButtonMenuMania != null && !isGameOver && (ShopWindow == null || !ShopWindow.activeSelf) && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (HomeButtonMenu == null || !HomeButtonMenu.activeSelf))
        {
            //SettingsButtonMenu.SetActive(true);
            SettingsButtonMenuMania.SetActive(!SettingsButtonMenuMania.activeSelf);
            OnMenuOpen(new MenuOpenEventArgs(SettingsButtonMenuMania.activeSelf));
        }
    }
    public void TycoonOpenSettingsButtonMenu()
    {
        if (SettingsButtonMenuMania != null && !isGameOver && (ShopWindow == null || !ShopWindow.activeSelf) && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (HomeButtonMenu == null || !HomeButtonMenu.activeSelf))
        {
            //SettingsButtonMenu.SetActive(true);
            SettingsButtonMenuTycoon.SetActive(!SettingsButtonMenuTycoon.activeSelf);
            OnMenuOpen(new MenuOpenEventArgs(SettingsButtonMenuTycoon.activeSelf));
        }
    }
    public void OpenHomeButtonMenu()
    {
        if (HomeButtonMenu != null && !isGameOver && (ShopWindow == null || !ShopWindow.activeSelf) && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (SettingsButtonMenuMania == null || !SettingsButtonMenuMania.activeSelf) && (SettingsButtonMenuTycoon == null || !SettingsButtonMenuTycoon.activeSelf))
        {
            //HomeButtonMenu.SetActive(true);
            HomeButtonMenu.SetActive(!HomeButtonMenu.activeSelf);
            OnMenuOpen(new MenuOpenEventArgs(HomeButtonMenu.activeSelf));
        }
    }
    //Timed Home Button for both modes
    public void OpenTimedHomeButtonMenu()
    {
        if ((HatchSettingsButtonMenu == null || !HatchSettingsButtonMenu.activeSelf) && (ProtectSettingsButtonMenu == null || !ProtectSettingsButtonMenu.activeSelf))
        {
            //TimedHomeButtonMenu.SetActive(true);
            TimedHomeButtonMenu.SetActive(!HatchSettingsButtonMenu.activeSelf);
            //OnMenuOpen(new MenuOpenEventArgs(TimedHomeButtonMenu.activeSelf)); // Do not want to pause on timed mode
        }
    }
    //Timed Hatch (not used)
    public void OpenHatchTimedSettingsButtonMenu()
    {
        if (TimedHomeButtonMenu == null || !TimedHomeButtonMenu.activeSelf)
        {
            //HatchSettingsButtonMenu.SetActive(true);
            HatchSettingsButtonMenu.SetActive(!HatchSettingsButtonMenu.activeSelf);
            //OnMenuOpen(new MenuOpenEventArgs(HatchSettingsButtonMenu.activeSelf)); // Do not want to pause on timed mode
        }
    }
    //Timed Protect (not used)
    public void OpenProtectTimedSettingsButtonMenu()
    {
        if (TimedHomeButtonMenu == null || !TimedHomeButtonMenu.activeSelf)
        {
            //HatchSettingsButtonMenu.SetActive(true);
            ProtectSettingsButtonMenu.SetActive(!ProtectSettingsButtonMenu.activeSelf);
            //OnMenuOpen(new MenuOpenEventArgs(ProtectSettingsButtonMenu.activeSelf)); // Do not want to pause on timed mode
        }
    }
    public void CloseAllWindows()
    {
        if (ShopWindow != null) ShopWindow.SetActive(false);
        if (UpgradeWindow != null) UpgradeWindow.SetActive(false);
        if (SettingsButtonMenuMania != null) SettingsButtonMenuMania.SetActive(false);
        if (SettingsButtonMenuTycoon != null) SettingsButtonMenuTycoon.SetActive(false);
        if (HomeButtonMenu != null) HomeButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));

        //Cancel all invokes from shopmanager. This is meant to stop the delayed pause event after buying a chicken. If you add another Invoked method to shopmanager and it breaks, this is why
        CancelInvoke();
    }
    private void OnUserInteraction(object sender, System.EventArgs e)
    {
        if (ShopWindow.activeSelf)
        {
            ShopWindow.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (UpgradeWindow.activeSelf)
        {
            UpgradeWindow.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (TimedHomeButtonMenu.activeSelf)
        {
            TimedHomeButtonMenu.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (HatchSettingsButtonMenu.activeSelf)
        {
            HatchSettingsButtonMenu.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (ProtectSettingsButtonMenu.activeSelf)
        {
            ProtectSettingsButtonMenu.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (HomeButtonMenu.activeSelf)
        {
            HomeButtonMenu.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (SettingsButtonMenuMania.activeSelf)
        {
            SettingsButtonMenuMania.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

        if (SettingsButtonMenuTycoon.activeSelf)
        {
            SettingsButtonMenuTycoon.SetActive(false);
            OnMenuOpen(new MenuOpenEventArgs(false));
            CancelInvoke();
        }

    }
    public bool IsSellZoneActive()
    {
        return activeSellZone != null;
    }
    /****************************************************************/

    /***************** Below counts the entity *********************/

    /*** Chicken Handler ***/
    public void AddChicken()
    {
        chickensCount++;
        UpdateUI();
    }


    /*** Chicks Handler ***/
    public void AddChick()
    {
        chicksCount++;
        UpdateUI();
    }
    public void ChickGrowsToChicken()
    {
        if (chicksCount > 0)
        {
            chicksCount--;
            chickensCount++;
            UpdateUI();
        }
    }
    /*** Egg Handler ***/
    public void AddEgg()
    {
        eggsCount++;
        UpdateUI();
    }
    public void HatchEgg()
    {
        if (eggsCount > 0)
        {
            eggsCount--;
            AddChick();
            UpdateUI();
        }
    }
    /*** Selling ***/
    public void SellEgg()
    {
        if (eggsCount > 0)
        {
            eggsCount--;
            UpdateUI();
        }
    }
    public void SellChick()
    {
        if (eggsCount > 0)
        {
            chicksCount--;
            UpdateUI();
        }
    }
    public void SellChicken()
    {
        if (chickensCount > 0)
        {
            chickensCount--;
            UpdateUI();
        }
    }
    /*** Eaten ***/
    public void LoseEgg()
    {
        if (eggsCount > 0)
        {
            eggsCount--;
            UpdateUI();
        }
    }
    public void LoseChick()
    {
        if (eggsCount > 0)
        {
            chicksCount--;
            UpdateUI();
        }
    }
    public void LoseChicken()
    {
        if (chickensCount > 0)
        {
            chickensCount--;
            UpdateUI();
        }
    }
    /*** Chicks to Chicken ***/
    public void UpdateUI()
    {
        //ChickensCount_Text.text = "Chickens: " + chickensCount;
        //ChicksCount_Text.text = "Chicks: " + chicksCount;
        //EggsCount_Text.text = "Eggs: " + eggsCount;

        Money_Text.text = "$" + Money.ToString();
        ShopMoneyText.text = "$" + Money.ToString();
        ShopUpgradeText.text = "$" + Money.ToString();
    }

    /****************************************************************/

        /***************** Below handles game over **********************/
    public GameObject GameOverWindow;
    private bool isGameOver = false; //To prevent multiple triggers
    private void CheckGameOver()
    {
        if (isGameOver) return;

        int totalChickens = chickensCount;
        int totalChicks = chicksCount;
        int totalEggs = eggsCount;
        int currentMoney = Money;
        int lowestPrice = Inventory[2, 1]; //Get the price of the cheapest chicken

        if (totalChickens == 0 && totalChicks == 0 && totalEggs == 0 && currentMoney < lowestPrice)
        {
            StartCoroutine(TriggerGameOver());
        }
    }
    private IEnumerator TriggerGameOver()
    {
        CloseAllWindows();
        isGameOver = true;
        if (GameOverWindow != null)
        {
            GameOverWindow.SetActive(true);
            int timeLeft = 30;
            while(timeLeft > -1)
            {
                GameOverText.text = $"You have no more money to buy Chickens!\r\nYou have 0 Chickens!\r\nReturning to Main Menu in {timeLeft} seconds...";
                yield return new WaitForSeconds(1f);
                timeLeft--;
            }
            if (timeLeft <= 0)
            {
                Destroy(activeSellZone);
                activeSellZone = null;
                ResetGame();
                GameOverWindow.SetActive(false);
            }
            Destroy(activeSellZone);
            activeSellZone = null;
        }
    }
    public void onReturnButton()
    {
        ResetGame();
        GameOverWindow.SetActive(false);
    }
    public void onReturnButtonTimerMode()
    {
        ResetTimerMode();
        Score.gameObject.SetActive(false);
    }
    private void ShowMoneyIndicator(int moneyEarned, GameObject buttonRef)
    {
        GameObject indicator = Instantiate(moneyIndicator, buttonRef.transform.position + Vector3.up * .8f, Quaternion.Euler(45, 0, 0));
        indicator.transform.SetParent(transform);
        TMPro.TMP_Text textComponent = indicator.GetComponentInChildren<TMPro.TMP_Text>();
        textComponent.text = $"-{moneyEarned}";
        textComponent.color = Color.red;
        Destroy(indicator, .5f);
    }

    /***************************************** Below handles Timer Mode *****************************************/

    private Coroutine startingTimedModeCoroutine;
    private Coroutine countdownRoutineCoroutine;
    private Coroutine callResetTimerModeCoroutine;
    public void StartCountdown()
    {
        startingTimedModeCoroutine = StartCoroutine(StartingTimedMode());
    }
    private IEnumerator StartingTimedMode()
    {

        int timeLeft = 3; // Starting countdown
        while (timeLeft > 0)
        {
            HatchStartingMessage.SetActive(true);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        HatchStartingMessage.SetActive(false);

        // Randomly spawn 10 chickens
        for (int i = 0; i < 10; i++)  // need above small delay for fox director
        {
            int itemId = Random.Range(1, 7); // Randomly select an item ID between 1 and 6
            if (itemId >= 1 && itemId <= 6)
            {
                SpawnChicken(itemId);
                AddChicken();
            }
        }

        tutorialTextPGM.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        timeLeft = 5;
        while (timeLeft > 0)
        {
            tutorialTextPGM.text = $"Starting in {timeLeft}!";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        tutorialTextPGM.text = "GO!";
        yield return new WaitForSeconds(1f);
        tutorialTextPGM.gameObject.SetActive(false);

        countdownRoutineCoroutine = StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        Timer = 45f;
        CountdownText.gameObject.SetActive(true);
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
            if (chickensCount == 0 && chicksCount == 0 && eggsCount == 0)
            {
                DisplayScore();
                yield break;
            }
        }

        Timer = 0;
        DisplayScore();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);
        CountdownText.text = $"{minutes:D2}:{seconds:D2}"; // Formats as MM:SS
    }
    void DisplayScore()
    {
        CountdownText.gameObject.SetActive(false);

        int Davin = 86;
        int placeholder1 = 70;
        int placeholder2 = 60;
        int placeholder3 = 50;
        int totalScore = chickensCount + chicksCount + eggsCount;

        // Destroy objects on the screen section
        Transform screenSectionTransform = screenSection.transform;
        GameObject[] draggableObjects = screenSectionTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("Draggable"))
            .Select(t => t.gameObject)
            .ToArray();

        foreach (GameObject obj in draggableObjects)
        {
            Destroy(obj);
        }

        // Destroy Foxes
        GameObject[] foxesToDestroy = GameObject.FindGameObjectsWithTag("Fox_" + screenSection.name);
        foreach (GameObject fox in foxesToDestroy) { Destroy(fox); }

        //Displays Score
        if (chickensCount == 0 && chicksCount == 0 && eggsCount == 0)
        {
            Score.text = $"\nYou lost all your chickens!";
        }
        else
        {
            if (totalScore > Davin)
            {
                Score.text = $"Time's up!\nYour Score: {totalScore}!\nPlaces you...\n1. YOU: {totalScore}.\n" +
                              $"2. Davin: {Davin}\n3. placeholder1: {placeholder1}\n" +
                              $"4. placeholder2: {placeholder2}\n5. placeholder3: {placeholder3}";
                // Display Confetti?
            }
            else if (totalScore > placeholder1 && totalScore <= Davin)
            {
                Score.text = $"Time's up!\nYour Score: {totalScore}!\nPlaces you...\n1. Davin: {Davin}\n" +
                              $"2. YOU: {totalScore}\n3. placeholder1: {placeholder1}\n" +
                              $"4. placeholder2: {placeholder2}\n5. placeholder3: {placeholder3}";
            }
            else if (totalScore > placeholder2 && totalScore <= placeholder1)
            {
                Score.text = $"Time's up!\nYour Score: {totalScore}!\nPlaces you...\n1. Davin: {Davin}\n" +
                              $"2. placeholder1: {placeholder1}\n3. YOU: {totalScore}\n" +
                              $"4. placeholder2: {placeholder2}\n5. placeholder3: {placeholder3}";
            }
            else if (totalScore > placeholder3 && totalScore <= placeholder2)
            {
                Score.text = $"Time's up!\nYour Score: {totalScore}!\nPlaces you...\n1. Davin: {Davin}\n" +
                              $"2. placeholder1: {placeholder1}\n3. placeholder2: {placeholder2}\n" +
                              $"4. YOU: {totalScore}\n5. placeholder3: {placeholder3}";
            }
            else
            {
                Score.text = $"Time's up!\nYour Score: {totalScore}!\nPlaces you...\n1. Davin: {Davin}\n" +
                              $"2. placeholder1: {placeholder1}\n3. placeholder2: {placeholder2}\n" +
                              $"4. placeholder3: {placeholder3}\n5. YOU: {totalScore}";
            }
        }

        Score.gameObject.SetActive(true);
        callResetTimerModeCoroutine = StartCoroutine(CallResetTimerMode());
    }

    /***************************************************** Below handles Protect Game Mode ****************************************************/

    private Coroutine startingDefendModeCoroutine;
    private Coroutine startingCountDownCoroutine;
    private Coroutine startingFoxSpawnCoroutine;

    public void StartCountdownPGM()
    {
        startingDefendModeCoroutine = StartCoroutine(StartingPGM());
    }
    private IEnumerator StartingPGM()
    {
        FoxDir.devourCooldown = 1;
        int timeLeft = 3; // Starting countdown
        while (timeLeft > 0)
        {
            DefendStartingMessage.SetActive(true);
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        DefendStartingMessage.SetActive(false);


        tutorialTextPGM.gameObject.SetActive(true);
        timeLeft = 5;
        while (timeLeft > 0)
        {
            tutorialTextPGM.text = $"Starting in {timeLeft} seconds...";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        tutorialTextPGM.text = "GO!";
        yield return new WaitForSeconds(1f);
        tutorialTextPGM.gameObject.SetActive(false);

        int itemId = Random.Range(1, 7); // Randomly select an item ID between 1 and 6
        SpawnChicken(itemId);
        AddChicken();
        NewChickenAI newChickenAI = lastSpawnedChicken.GetComponent<NewChickenAI>();
        newChickenAI.foxDetectionRadius = 0.6f;

        startingCountDownCoroutine = StartCoroutine(CountdownRoutinePGM());
        startingFoxSpawnCoroutine = StartCoroutine(UpdateFoxesPer5ChickensRoutine());
    }

    private IEnumerator CountdownRoutinePGM()
    {
        Timer = 0f;
        CountdownText.gameObject.SetActive(true);
        while (chickensCount > 0)
        {
            Timer += Time.deltaTime;
            UpdateTimerDisplayPGM();
            yield return null;
        }

        DisplayScorePGM();
    }
    private IEnumerator UpdateFoxesPer5ChickensRoutine()
    {
        int foxesToSpawn = 1;

        while (Timer > 0)
        {
            yield return new WaitForSeconds(5f);
            if (chickensCount == 1)
            {
                //FoxDir.foxesPer5Chickens += 5f; // Does not work because int conversion = 0
                for (int i = 0; i < foxesToSpawn; i++)
                {
                    FoxDir.SpawnFox();
                    yield return new WaitForSeconds(0.4f); // Small delay between each fox spawn (0.5 seconds)
                }
                foxesToSpawn += 2;
            }
            else
            {
                yield return new WaitForSeconds(5f);
            }
        }
    }
    void UpdateTimerDisplayPGM()
    {
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);
        CountdownText.text = $"{minutes:D2}:{seconds:D2}"; // Formats as MM:SS
    }
    void DisplayScorePGM()
    {
        CountdownText.gameObject.SetActive(false);
        if (startingFoxSpawnCoroutine != null)
        {
            StopCoroutine(startingFoxSpawnCoroutine);
            startingFoxSpawnCoroutine = null;
        }
        // Destroy objects on the screen section
        Transform screenSectionTransform = screenSection.transform;
        GameObject[] draggableObjects = screenSectionTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("Draggable"))
            .Select(t => t.gameObject)
            .ToArray();

        foreach (GameObject obj in draggableObjects)
        {
            Destroy(obj);
        }

        // Destroy Foxes
        GameObject[] foxesToDestroy = GameObject.FindGameObjectsWithTag("Fox_" + screenSection.name);
        foreach (GameObject fox in foxesToDestroy) { Destroy(fox); }

        //Resets the values to before game started
        //FoxDir.spawnTick = 10;
        //FoxDir.foxesPer5Chickens = 1f;

        if (chickensCount == 0)
        {
            int minutes = Mathf.FloorToInt(Timer / 60);
            int seconds = Mathf .FloorToInt(Timer % 60);
            Score.text = $"You lost your chicken!\nTime Survived: {minutes:D2}:{seconds:D2}";
        }

        Score.gameObject.SetActive(true);
        callResetTimerModeCoroutine = StartCoroutine(CallResetTimerMode());
    }

    private IEnumerator CallResetTimerMode()
    {
        yield return new WaitForSeconds(20f);
        ResetTimerMode();
    }


    /*************************************************** Reset Game State function *************************************************/

    public void ResetGame()
    {
        // Destroy objects on the screen section
        Transform screenSectionTransform = screenSection.transform;
        GameObject[] draggableObjects = screenSectionTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("Draggable"))
            .Select(t => t.gameObject)
            .ToArray();

        foreach (GameObject obj in draggableObjects)
        {
            Destroy(obj);
        }

        // Destroy Foxes
        GameObject[] foxesToDestroy = GameObject.FindGameObjectsWithTag("Fox_" + screenSection.name);
        foreach (GameObject fox in foxesToDestroy) { Destroy(fox); }

        Money = 100;
        chickensCount = 0;
        chicksCount = 0;
        eggsCount = 0;
        FoxDir.devourCooldown = 5;

        //Resets Array's Upgrade Cost
        Inventory[2, 8] = 30;
        Inventory[2, 9] = 25;
        Inventory[2, 10] = 25;
        Inventory[2, 11] = 50;
        //Resets Array's Upgrade Count
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;
        Inventory[3, 11] = 0;
        //Resets Array's Chicken Counts
        Inventory[3, 1] = 0;
        Inventory[3, 2] = 0;
        Inventory[3, 3] = 0;
        Inventory[3, 4] = 0;
        Inventory[3, 5] = 0;
        Inventory[3, 6] = 0;
        Inventory[3, 7] = 0;
        
        GameOverWindow.SetActive(false);
        isGameOver = false;

        Destroy(activeSellZone);
        activeSellZone = null;
        UpdateUI();

        // Call the ReturnToTitlePage function
        if (screenController != null)
        {
            screenController.ReturnToTitlePage();
        }
        else
        {
            Debug.LogWarning("ScreenController not found on " + screenSection.name);
        }
    }
    /*
    * Reset Game State function
    */
    public void ResetTimerMode()
    {
        // Destroy objects on the screen section
        Transform screenSectionTransform = screenSection.transform;
        GameObject[] draggableObjects = screenSectionTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("Draggable"))
            .Select(t => t.gameObject)
            .ToArray();

        foreach (GameObject obj in draggableObjects)
        {
            Destroy(obj);
        }

        // Destroy Foxes
        GameObject[] foxesToDestroy = GameObject.FindGameObjectsWithTag("Fox_" + screenSection.name);
        foreach (GameObject fox in foxesToDestroy) { Destroy(fox); }

        Money = 100;
        chickensCount = 0;
        chicksCount = 0;
        eggsCount = 0;
        GoldEggChance = 100;
        //Resets Array's Upgrade Cost
        Inventory[2, 8] = 30;
        Inventory[2, 9] = 25;
        Inventory[2, 10] = 25;
        Inventory[2, 11] = 50;
        //Resets Array's Upgrade Count
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;
        Inventory[3, 11] = 0;

        FoxDir.graceTime = 10;

        HatchStartingMessage.SetActive(false);
        DefendStartingMessage.SetActive(false);
        tutorialTextPGM.gameObject.SetActive(false);

        //Reset UI elements
        UpdateUI();
        CountdownText.gameObject.SetActive(false);
        Score.gameObject.SetActive(false);
        StopCoroutine(CallResetTimerMode());
        StopCoroutine(CountdownRoutine());
        StopCoroutine(StartingTimedMode());
        StopCoroutine(StartingPGM());
        UpdateTimerDisplay();

        if (startingTimedModeCoroutine != null)
        {
            StopCoroutine(startingTimedModeCoroutine);
            startingTimedModeCoroutine = null; 
        }
        if (countdownRoutineCoroutine != null)
        {
            StopCoroutine(countdownRoutineCoroutine);
            countdownRoutineCoroutine = null;
        }
        
        if (startingDefendModeCoroutine != null)
        {
            StopCoroutine(startingDefendModeCoroutine);
            startingDefendModeCoroutine = null; 
        }
        if (startingCountDownCoroutine != null)
        {
            StopCoroutine(startingCountDownCoroutine);
            startingCountDownCoroutine = null;
        }
        if (startingFoxSpawnCoroutine != null)
        {
            StopCoroutine(startingFoxSpawnCoroutine);
            startingFoxSpawnCoroutine = null;
        }
        if (callResetTimerModeCoroutine != null)
        {
            StopCoroutine(callResetTimerModeCoroutine);
            callResetTimerModeCoroutine = null;
        }

        // Call the ReturnToTitlePage function
        if (screenController != null)
        {
            screenController.ReturnToTitlePage();
        }
    }
    /****************************************************************/

    /***For Close Settings and Close Home Button Menu***/
    public void ManiaCloseSettingsButtonMenu()
    {
        SettingsButtonMenuMania.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }
    public void TycoonCloseSettingsButtonMenu()
    {
        SettingsButtonMenuTycoon.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }
    public void CloseHomeButtonMenu()
    {
        HomeButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }

    /*For Timed Gamemode*/


    public void CloseTimedSettingsButtonMenu()
    {
        HatchSettingsButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }

    public void CloseTimedHomeButtonMenu()
    {
        TimedHomeButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }

    //Inactivity
    private void RegisterTouchGestures()
    {
        if (Screen != null)
        {
            TapGesture tapGesture = Screen.GetComponent<TapGesture>();
            PressGesture pressGesture = Screen.GetComponent<PressGesture>();

            if (tapGesture != null)
            {
                tapGesture.Tapped += OnUserActivity;
            }

            if (pressGesture != null)
            {
                pressGesture.Pressed += OnUserActivity;
            }
        }

        if (hudObject != null)
        {
            TapGesture tapGesture = hudObject.GetComponent<TapGesture>();
            PressGesture pressGesture = hudObject.GetComponent<PressGesture>();

            if (tapGesture != null)
            {
                tapGesture.Tapped += OnUserActivity;
            }

            if (pressGesture != null)
            {
                pressGesture.Pressed += OnUserActivity;
            }
        }

    }

    private void OnUserActivity(object sender, System.EventArgs e)
    {
        ResetInactivityTimer();
    }

    public void ResetInactivityTimer()
    {
        lastInteractionTime = Time.time;

        if (inactivityWarningGreen != null)
        {
            inactivityWarningGreen.SetActive(false);
        }

        if (inactivityWarningOrange != null)
        {
            inactivityWarningOrange.SetActive(false);
        }

        countdownStarted = false;
    }

    private void ShowInactivityWarning()
    {
        if (!countdownStarted)
        {
            bool showGreen = Random.value < 0.5f;

            if (showGreen && inactivityWarningGreen != null)
            {
                inactivityWarningGreen.SetActive(true);

                if (inactivityWarningOrange != null)
                {
                    inactivityWarningOrange.SetActive(false);
                }
            }
            else if (!showGreen && inactivityWarningOrange != null)
            {
                inactivityWarningOrange.SetActive(true);

                if (inactivityWarningGreen != null)
                {
                    inactivityWarningGreen.SetActive(false);
                }
            }

            countdownStarted = true;
            countdownTime = returnToMenuTime;
        }
    }
}



public class MenuOpenEventArgs : EventArgs
{
    public bool State { get; set; }

    public MenuOpenEventArgs(bool state)
    { 
        this.State = state;
    }
}
