using System.Collections;
using UnityEngine;

public class Sell : MonoBehaviour
{
    [SerializeField]
    private string dropZoneTag = "DropZone";

    public ShopManager shopManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(dropZoneTag))
        {
            GiveMoney(gameObject);
            Destroy(gameObject);
        }
    }

    private void GiveMoney(GameObject droppedObject)
    {
        int moneyEarned = 0;

        if (droppedObject.name.Contains("rhode"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 2 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("leghorn"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 2 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 3 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("astralorp"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 3 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("silkie"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("polish"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("easter"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 7 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (droppedObject.name.Contains("chicken"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 50000 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }

        if (moneyEarned > 0)
        {
            shopManager.Money += moneyEarned;
            shopManager.Money_Text.text = shopManager.Money.ToString();
        }
    }
}
