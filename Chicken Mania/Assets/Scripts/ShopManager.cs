
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

public class ShopManager : MonoBehaviour
{
    public int[,] Inventory = new int[4,12]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    //public int startingMoney = 100000;
    public TextMeshProUGUI Money_Text;
    public TextMeshProUGUI ChickensCount_Text;
    public TextMeshProUGUI ChicksCount_Text;
    public TextMeshProUGUI EggsCount_Text;
    public TextMeshProUGUI WinMessage;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI Score;

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

    void Start()
    {
        screenController = screenSection.GetComponent<ScreenController>();
        Money_Text.text = Money.ToString();
        UpdateUI();

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
        Inventory[1, 11] = 11; //Secret Chicken

        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[1, 7] = 7;
        Inventory[1, 8] = 8;
        Inventory[1, 9] = 9;
        Inventory[1, 10] = 10;


        /*--------------------------------------------------------------------------
         *************************************************************************** 
         ******************************* Pricing ***********************************
         ***************************************************************************
         --------------------------------------------------------------------------*/

        //Chicken Price
        Inventory[2, 1] = 20;
        Inventory[2, 2] = 30;
        Inventory[2, 3] = 75;
        Inventory[2, 4] = 113;
        Inventory[2, 5] = 281;
        Inventory[2, 6] = 600;
        Inventory[2, 11] = 10000; //Secret Chicken

        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[2, 7] = 30;
        Inventory[2, 8] = 10;
        Inventory[2, 9] = 15;
        Inventory[2, 10] = 50;


        /*--------------------------------------------------------------------------
         *************************************************************************** 
         ******************************** Count ************************************
         ***************************************************************************
         --------------------------------------------------------------------------*/

        //Chicken Count (NOT USED)
        Inventory[3, 1] = 0;
        Inventory[3, 2] = 0;
        Inventory[3, 3] = 0;
        Inventory[3, 4] = 0;
        Inventory[3, 5] = 0;
        Inventory[3, 6] = 0;
        Inventory[3, 11] = 0;

        //Upgrades Count (Supplements, Feed, Incubator, Research)
        Inventory[3, 7] = 0;
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;

    }
    void Update()
    {
        CheckGameOver();
    }

    public void Buy()
    {
        //References to the button clicked
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int itemId = ButtonRef.GetComponent<ShopButtons>().ItemID;

        //Check if enough money is available for the purchase
        if (Money >= Inventory[2, itemId])
        {
            //Deduct money and update UI
            Money -= Inventory[2, itemId];
            //Money_Text.text = Money.ToString();

            //Increment the count for the item (upgrade)
            Inventory[3, itemId] += 1;

            //If buying a chicken (Index [x,1-6]), spawn it
            if (itemId >= 1 && itemId <= 6)
            {
                SpawnChicken(itemId);
                AddChicken();
            }
            // Show message if itemId 6 is bought
            if (itemId == 6)
            {
                StartCoroutine(ShowMessage("You have purchased the ultimate chicken! You Won! Continue playing or time out to reset!"));
            }

            //Only applies multiplier to Upgrade indexes
            if (itemId >= 7 && itemId <= 10)
            {
                //Recalculate the price: Price = BasePrice * (Count + 1)
                //Inventory[2, itemId] = Inventory[2, itemId] * (Inventory[3, itemId] + 1);

                //Recalculate the price: Price = BasePrice * 1.1^(Count + 1)
                Inventory[2, itemId] = Mathf.RoundToInt(Inventory[2, itemId] * Mathf.Pow(1.6f, Inventory[3, itemId] + 1));

                /*
                // Disable the button if the count reaches 3
                if (Inventory[3, itemId] >= 3)
                {
                    ButtonRef.GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
                */
            }
        }
    }
    public void SpawnChicken(int itemId)
    {
        int index = itemId - 1;
        if (index >= 0 && index < ChickenSpecies.Length && ChickenSpecies[index] != null && SpawnPoint != null)
        {
            GameObject newChicken = Instantiate(ChickenSpecies[index], SpawnPoint.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

            // Set chicken as child of screen
            newChicken.transform.SetParent(screenSection.transform);

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
    private IEnumerator ShowMessage(string message)
    {
        WinMessage.text = message;
        WinMessage.gameObject.SetActive(true);
        TimerText.gameObject.SetActive(true);
        for (int i = 10; i > 0; i--)
        {
            TimerText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        WinMessage.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(false);
    }

    /**************** Below is to Toggle the shop menu ****************/

    public void ToggleSell()
    {
        if (dragZone != null)
        {
            dragZone.SetActive(!dragZone.activeSelf); //Toggle the DropZone/Sell
        }
    }

    public void ToggleBuy()
    {
        if (ShopWindow != null)
        {
            ShopWindow.SetActive(!ShopWindow.activeSelf); //Toggle the Chicken Shop Window
        }
    }

    public void ToggleUpgrade()
    {
        if (UpgradeWindow != null)
        {
            UpgradeWindow.SetActive(!UpgradeWindow.activeSelf); //Toggle the upgrade shop
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
    private void UpdateUI()
    {
        ChickensCount_Text.text = "Chickens: " + chickensCount;
        ChicksCount_Text.text = "Chicks: " + chicksCount;
        EggsCount_Text.text = "Eggs: " + eggsCount;

        Money_Text.text = Money.ToString();
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
            TriggerGameOver();
        }
    }
    private void TriggerGameOver()
    {
        isGameOver = true;
        if (GameOverWindow != null)
        {
            GameOverWindow.SetActive(true);
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

    /* Below handles Timer Mode */
    public void StartCountdown()
    {
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

    /*
     * Reset Game State function
     */
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
        Inventory[3, 7] = 0;
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;
        Inventory[2, 7] = 30;
        Inventory[2, 8] = 10;
        Inventory[2, 9] = 15;
        Inventory[2, 10] = 50;

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
        chickensCount = 0;
        chicksCount = 0;
        eggsCount = 0;
        Inventory[3, 7] = 0;
        Inventory[3, 8] = 0;
        Inventory[3, 9] = 0;
        Inventory[3, 10] = 0;
        Inventory[2, 7] = 30;
        Inventory[2, 8] = 10;
        Inventory[2, 9] = 15;
        Inventory[2, 10] = 50;
        Timer = 120f;

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
}
