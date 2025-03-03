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
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.25f)+ ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.5f) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.7f) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 8]*0.1f))));
                shopManager.SellChicken();
                Destroy(droppedObject);

            }
        }
        else if (droppedObject.name.Contains("leghorn"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.25f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.5f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.7f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("astralorp"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.25f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.5f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.7f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("easter"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.25f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.5f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.7f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("silkie"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.25f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.5f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.7f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellChicken();
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("polish"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.25f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.5f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 8] * 0.1f))));
                shopManager.SellEgg();
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.7f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 8] * 0.1f))));
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
