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

    public void GiveMoney(GameObject droppedObject)
    {
        int moneyEarned = 0;

        if (droppedObject.name.Contains("rhode"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 3 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);

            }
        }
        else if (droppedObject.name.Contains("leghorn"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 7 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("astralorp"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 8 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 10 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("easter"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 8 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 10 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 12 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("silkie"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 10 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 12 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 14 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("polish"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = 12 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = 14 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = 16 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }

        if (moneyEarned > 0)
        {
            shopManager.Money += moneyEarned;
            shopManager.Money_Text.text = shopManager.Money.ToString();
        }
    }
}
