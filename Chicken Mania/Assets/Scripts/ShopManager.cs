using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int[,] Inventory = new int[4,11]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    public TextMeshProUGUI Money_Text;
    public TextMeshProUGUI ChickensCount_Text;
    public TextMeshProUGUI ChicksCount_Text;
    public TextMeshProUGUI EggsCount_Text;

    //Chicken Species & Spawn
    public GameObject[] ChickenSpecies;
    public Transform SpawnPoint;

    public int chickensCount = 0;
    public int chicksCount = 0;
    public int eggsCount = 0;

    public GameObject dragZone;
    public GameObject ShopWindow;
    public GameObject UpgradeWindow;

    void Start()
    {
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
        Inventory[2, 1] = 50;
        Inventory[2, 2] = 100;
        Inventory[2, 3] = 200;
        Inventory[2, 4] = 400;
        Inventory[2, 5] = 600;
        Inventory[2, 6] = 1000;

        //Upgrades (Supplements, Feed, Incubator, Research)
        Inventory[2, 7] = 100;
        Inventory[2, 8] = 100;
        Inventory[2, 9] = 100;
        Inventory[2, 10] = 100;


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
    /*
    private void Update()
    {
        //Check for click outside the UI windows (Shop and Upgrade)
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            //Create a PointerEventData to check where the click occurred
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            //List to hold the results of the raycast
            var results = new System.Collections.Generic.List<RaycastResult>();

            //Raycast to get all UI elements under the mouse
            EventSystem.current.RaycastAll(pointerEventData, results);

            bool clickedOnUI = false;

            foreach (var result in results)
            {
                //if click is inside window, don't close
                if (result.gameObject == ShopWindow || result.gameObject == UpgradeWindow)
                {
                    clickedOnUI = true;
                    break;
                }
            }
           
            if (!clickedOnUI) //Closes windows
            {
                if (ShopWindow.activeSelf)
                    ToggleBuy();

                if (UpgradeWindow.activeSelf)
                    ToggleUpgrade();
            }
        }
    }
    */
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
            Money_Text.text = Money.ToString();

            //Increment the count for the item (upgrade)
            Inventory[3, itemId] += 1;

            //If buying a chicken (Index [x,1-6]), spawn it
            if (itemId >= 1 && itemId <= 6)
            {
                SpawnChicken(itemId);
                AddChicken();
            }

                //Only applies multiplier to Upgrade indexes
                if (itemId >= 7 && itemId <= 10)
            {
                //Recalculate the price: Price = BasePrice * (Count + 1)
                Inventory[2, itemId] = Inventory[2, itemId] * (Inventory[3, itemId] + 1);

                // Disable the button if the count reaches 3
                if (Inventory[3, itemId] >= 3)
                {
                    ButtonRef.GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
            }
        }
    }
    void SpawnChicken(int itemId)
    {
        int index = itemId - 1;
        if (index >= 0 && index < ChickenSpecies.Length && ChickenSpecies[index] != null && SpawnPoint != null)
        {
            Instantiate(ChickenSpecies[index], SpawnPoint.position, Quaternion.identity);
        }
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
    /*** Chicks to Chicken ***/
    private void UpdateUI()
    {
        ChickensCount_Text.text = "Chickens: " + chickensCount;
        ChicksCount_Text.text = "Chicks: " + chicksCount;
        EggsCount_Text.text = "Eggs: " + eggsCount;
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
        SceneManager.LoadScene("Main Menu");
    }
    /****************************************************************/

}
