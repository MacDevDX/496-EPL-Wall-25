using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopButtons : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI LevelText;
    public Button ButtonComponent;
    public GameObject ShopManager;

    void Start()
    {
        UpdateButtonState();
    }

    void Update()
    {
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        var shopManager = ShopManager.GetComponent<ShopManager>();

        if (ItemID >= 7 && ItemID <= 10) //For upgrades
        {
            if (shopManager.Inventory[3, ItemID] >= 3)
            {
                LevelText.text = "Upgrade Max";
                PriceText.text = ""; //Hide the price
                ButtonComponent.interactable = false;
            }
            else
            {
                LevelText.text = "Upgrade: #" + shopManager.Inventory[3, ItemID].ToString();
                PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
                ButtonComponent.interactable = true;
            }
        }
        else if (ItemID >= 3 && ItemID <= 6) //Locking chicken tiers 3-6
        {
            int researchLevel = shopManager.Inventory[3, 10];

            if (ItemID == 3 || ItemID == 4) //Tiers 3 & 4 unlock at Research Level 1+
            {
                if (researchLevel >= 1)
                {
                    PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID];
                    ButtonComponent.interactable = true;
                }
                else
                {
                    PriceText.text = "Requires Research 1";
                    ButtonComponent.interactable = false;
                }
            }
            else if (ItemID == 5) //Tier 5 unlocks at Research Level 2+
            {
                if (researchLevel >= 2)
                {
                    PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID];
                    ButtonComponent.interactable = true;
                }
                else
                {
                    PriceText.text = "Requires Research 2";
                    ButtonComponent.interactable = false;
                }
            }
            else if (ItemID == 6) //Tier 6 unlocks at Research Level 3
            {
                if (researchLevel >= 3)
                {
                    PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID];
                    ButtonComponent.interactable = true;
                }
                else
                {
                    PriceText.text = "Requires Research 3";
                    ButtonComponent.interactable = false;
                }
            }
        }
        /*
        else if (ItemID == 11) //Secret Chicken
        {
            if (shopManager.Inventory[3, 6] >= 1) //If has Tier 6 chicken: Show
            {
                PriceText.gameObject.SetActive(true);
                ButtonComponent.gameObject.SetActive(true);
                PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID];
                ButtonComponent.interactable = true;
            }
            else //If no tier 6: Don't show
            {
                PriceText.gameObject.SetActive(false);
                ButtonComponent.gameObject.SetActive(false);
            }
        }
        */
        else //For normal shop items
        {
            PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
            ButtonComponent.interactable = true;
        }
    }
}

