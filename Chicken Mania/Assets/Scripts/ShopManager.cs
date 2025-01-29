using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public int[,] Inventory = new int[4,11]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    public TextMeshProUGUI Money_Text;
    //Chicken Species & Spawn
    public GameObject[] ChickenSpecies;
    public Transform SpawnPoint;

    void Start()
    {
        Money_Text.text = Money.ToString();

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
            if (itemId >= 1 && itemId <=6)
            {
                SpawnChicken(itemId);
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
        if (index >= 1 && index < ChickenSpecies.Length && ChickenSpecies[index] != null && SpawnPoint != null)
        {
            Instantiate(ChickenSpecies[index], SpawnPoint.position, Quaternion.identity);
        }
    }
 }
