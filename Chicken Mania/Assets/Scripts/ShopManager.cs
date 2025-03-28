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
using UnityEngine.WSA;

public class ShopManager : MonoBehaviour
{
    public int[,] Inventory = new int[4,12]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    //public int startingMoney = 100000;
    public TextMeshProUGUI Money_Text;
    //public TextMeshProUGUI ChickensCount_Text;
    //public TextMeshProUGUI ChicksCount_Text;
    //public TextMeshProUGUI EggsCount_Text;
    public TextMeshProUGUI WinMessage;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI tutorialTextPGM;
    public TextMeshProUGUI GameOverText;

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

    private float Timer = 120f;
    public float timeToGrow = 10f;
    public float timeToSpawn = 10f;
    public float FoxDetection = 0f;
    public int GoldEggChance = 500;
    public GameObject lastSpawnedChicken;

    private TapGesture tapGesture;

    private GameObject activeSellZone;

    // Music and Home Button Menu
    [Header("Settings")]
    public GameObject SettingsButtonMenu;
    public GameObject HomeButtonMenu;

    // Pause Broadcast
    public delegate void MenuOpenEventHandler(object sender, MenuOpenEventArgs e);
    public event MenuOpenEventHandler MenuOpen;

    [Header("Theme Objects")]
    public GameObject ChristmasLights;
    public GameObject Jackolantern;

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
        Inventory[2, 1] = 20;
        Inventory[2, 2] = 100;
        Inventory[2, 3] = 500;
        Inventory[2, 4] = 3500;
        Inventory[2, 5] = 28000;
        Inventory[2, 6] = 300000;
        Inventory[2, 7] = 10000000; //Secret Chicken

        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[2, 8] = 300;
        Inventory[2, 9] = 3000;
        Inventory[2, 10] = 100;
        Inventory[2, 11] = 1000;


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

    }
    void Update()
    {
        CheckGameOver();
    }

    protected virtual void OnMenuOpen(MenuOpenEventArgs e)
    {
        MenuOpen?.Invoke(this, e);
    }

    public void Buy()
    {
        // a check for if money is less than the cheapest item, there is already a money check below
        // if (Money < Inventory[2, 1]) return;

        //References to the button clicked
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ButtonRef == null)
        {
            // When not enough money, ButtonRef doesnt exist. Button is probably disabled somehow when not enough money.
            FloatingMoneyText.SpawnText("Not Enough Money", new Vector3(-0.3f, 10.7f, -7.3f), Color.gray);
            return;
        }
        int itemId = ButtonRef.GetComponent<ShopButtons>().ItemID;

        //Check if enough money is available for the purchase
        if (Money >= Inventory[2, itemId])
        {
            //Deduct money and update UI
            Money -= Inventory[2, itemId];
            FloatingMoneyText.SpawnText(Inventory[2, itemId].ToString(), new Vector3(-0.3f, 10.7f, -7.3f), Color.red, "-");
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
                SpawnFriedChicken(itemId);
                AddChicken();
                //Pause after a short delay after buying chicken
                Invoke("RaisePauseEvent", 0.5f);
            }

            //Only applies multiplier to Upgrade indexes
            if (itemId == 8)
            {
                Inventory[2, itemId] = (Inventory[2, itemId] + (30 * Inventory[3, itemId]));
            }
            if (itemId == 9)
            {
                Inventory[2, itemId] = Mathf.RoundToInt(Inventory[2, itemId] * Mathf.Pow(1.6f, Inventory[3, itemId] + 1));
            }
            if (itemId == 10)
            {
                Inventory[2, itemId] = (Inventory[2, itemId] + 25);
            }
            if (itemId == 11)
            {
                //Recalculate the price: Price = BasePrice * 1.6^(Upgrade + 1)
                Inventory[2, itemId] = Mathf.RoundToInt(Inventory[2, itemId] * Mathf.Pow(1.6f, Inventory[3, itemId] + 1));
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
            AnimatedEggSpawner eggScript = newChicken.GetComponent<AnimatedEggSpawner>();
            if (eggScript != null)
            {
                eggScript.FoxDir = FoxDir;
                eggScript.shopManager = this;

                // Set egg as child of screen
                eggScript.transform.SetParent(screenSection.transform);
            }
            else
            {
                NewEggSpawner newEggScript = newChicken.GetComponent<NewEggSpawner>();
                newEggScript.FoxDir = FoxDir;
                newEggScript.shopManager = this;

                // Set the egg spawner under the screen
                newEggScript.transform.SetParent(screenSection.transform);
            }
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
        if (ShopWindow != null && !isGameOver && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (SettingsButtonMenu == null || !SettingsButtonMenu.activeSelf))
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
        if (UpgradeWindow != null && !isGameOver && (ShopWindow == null || !ShopWindow.activeSelf) && (SettingsButtonMenu == null || !SettingsButtonMenu.activeSelf))
        {
            UpgradeWindow.SetActive(!UpgradeWindow.activeSelf); //Toggle the upgrade shop
            OnMenuOpen(new MenuOpenEventArgs(UpgradeWindow.activeSelf));

            if (UpgradeWindow.activeSelf == false)
            {
                //Cancel all invokes from shopmanager. This is meant to stop the delayed pause event after buying a chicken. If you add another Invoked method to shopmanager and it breaks, this is why
                CancelInvoke();
            }
        }
    }
    public void CloseAllWindows()
    {
        if (ShopWindow != null) ShopWindow.SetActive(false);
        if (UpgradeWindow != null) UpgradeWindow.SetActive(false);
        if (SettingsButtonMenu != null) SettingsButtonMenu.SetActive(false);
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

    /***************************************** Below handles Timer Mode *****************************************/
    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
        StartCoroutine(StartingTimedMode());
    }
    private IEnumerator StartingTimedMode()
    {
        tutorialTextPGM.gameObject.SetActive(true);
        int timeLeft = 5; // Starting countdown
        while (timeLeft > 0)
        {
            tutorialTextPGM.text = $"Hatch the eggs! \nStarting in {timeLeft} seconds...";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        tutorialTextPGM.text = "GO!";
        yield return new WaitForSeconds(1f);
        tutorialTextPGM.gameObject.SetActive(false);

        // Randomly spawn 10 chickens
        for (int i = 0; i < 10; i++)
        {
            int itemId = Random.Range(1, 7); // Randomly select an item ID between 1 and 6
            if (itemId >= 1 && itemId <= 6)
            {
                SpawnChicken(itemId);
                AddChicken();
            }

        }

        StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
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

        //Displays Total Count
        Score.text = $"Time's up!\nYour Score: {chickensCount+chicksCount+eggsCount}!";
        Score.gameObject.SetActive(true);
    }

    /***************************************************** Below handles Protect Game Mode ****************************************************/
    public void StartCountdownPGM()
    {
        StartCoroutine(StartingPGM());
    }
    private IEnumerator StartingPGM()
    {
        tutorialTextPGM.gameObject.SetActive(true);
        int timeLeft = 5; // Starting countdown
        while (timeLeft > 0)
        {
            tutorialTextPGM.text = $"Protect the chicken from foxes! \nStarting in {timeLeft} seconds...";
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
        newChickenAI.foxDetectionRadius = 10f;

        StartCoroutine(CountdownRoutinePGM());
        StartCoroutine(UpdateFoxesPer5ChickensRoutine());
    }

    private IEnumerator CountdownRoutinePGM()
    {
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            UpdateTimerDisplayPGM();
            if (chickensCount == 0)
            {
                DisplayScorePGM();
            }

            yield return null;
        }

        Timer = 0;
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
                    yield return new WaitForSeconds(0.5f); // Small delay between each fox spawn (0.5 seconds)
                }
                foxesToSpawn += 2;
            }
            else yield return new WaitForSeconds(5f);
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
            Score.text = "You lost your chicken!";
        }
        else if (chickensCount >= 1)
        {
            Score.text = "Time's up!\nYour chicken survived!";
        }
        Score.gameObject.SetActive(true);
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

        Money = 20;
        chickensCount = 0;
        chicksCount = 0;
        eggsCount = 0;

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

        ChristmasLights.SetActive(false);
        Jackolantern.SetActive(false);
        
        GameOverWindow.SetActive(false);
        isGameOver = false;

        Destroy(activeSellZone);
        activeSellZone = null;
        UpdateUI();

        // Check if win message is on and stops it
        if (WinMessage != null )
        {
            WinMessage.gameObject.SetActive(false);
        }

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
        chickensCount = 0;
        chicksCount = 0;
        eggsCount = 0;
        GoldEggChance = 500;
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
        Timer = 120f;
        FoxDir.foxList.Clear();

        ChristmasLights.SetActive(false);
        Jackolantern.SetActive(false);

        //Reset UI elements
        UpdateUI();
        CountdownText.gameObject.SetActive(true);
        UpdateTimerDisplay();

        // Call the ReturnToTitlePage function
        if (screenController != null)
        {
            screenController.ReturnToTitlePage();
        }
    }
    /****************************************************************/

    /***For Settings and Home Button Menu***/
    public void OpenSettingsButtonMenu()
    {
        if ((ShopWindow == null || !ShopWindow.activeSelf) && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (HomeButtonMenu == null || !HomeButtonMenu.activeSelf))
        {
            SettingsButtonMenu.SetActive(true);
            OnMenuOpen(new MenuOpenEventArgs(SettingsButtonMenu.activeSelf));
        }
    }

    public void CloseSettingsButtonMenu()
    {
        SettingsButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
    }

    public void OpenHomeButtonMenu()
    {
        if ((ShopWindow == null || !ShopWindow.activeSelf) && (UpgradeWindow == null || !UpgradeWindow.activeSelf) && (SettingsButtonMenu == null || !SettingsButtonMenu.activeSelf))
        {
            HomeButtonMenu.SetActive(true);
            OnMenuOpen(new MenuOpenEventArgs(HomeButtonMenu.activeSelf));
        }
    }

    public void CloseHomeButtonMenu()
    {
        HomeButtonMenu.SetActive(false);
        OnMenuOpen(new MenuOpenEventArgs(false));
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
