using TMPro;
using UnityEngine;


public class ShopButtons : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI LevelText;
    public GameObject ShopManager;

    void Update()
    {
        //Retrieve the ShopManager component
        var shopManager = ShopManager.GetComponent<ShopManager>();

        //Only apply this to indexes 7-10
        if (ItemID >= 7 && ItemID <= 10)
        {
            //Check if the count is 3 or higher to display "Upgrade Max" and hide the price
            if (shopManager.Inventory[3, ItemID] >= 3)
            {
                LevelText.text = "Upgrade Max";
                PriceText.text = ""; // Hide the price
            }
            else
            {
                LevelText.text = "Upgrade: #" + shopManager.Inventory[3, ItemID].ToString();
                PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
            }
        }
        else
        {
            //For non-upgrade items, retain default behavior
            LevelText.text = "Upgrade: #" + shopManager.Inventory[3, ItemID].ToString();
            PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
        }
    }

}
