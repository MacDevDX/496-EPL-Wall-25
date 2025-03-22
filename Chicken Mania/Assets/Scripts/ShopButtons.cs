using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopButtons : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceText;
    //public TextMeshProUGUI LevelText;
    public Button ButtonComponent;
    public GameObject ShopManager;
    public GameObject SecretTier;

    [Header("Supplements Objects")]
    public GameObject S0;
    public GameObject S1;
    public GameObject S2;
    public GameObject S3;
    [Header("Feed Objects")]
    public GameObject F0;
    public GameObject F1;
    public GameObject F2;
    public GameObject F3;
    [Header("Incubator Objects")]
    public GameObject I0;
    public GameObject I1;
    public GameObject I2;
    public GameObject I3;
    [Header("Research Objects")]
    public GameObject R0;
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;

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

        if (ItemID == 1)
        {
            if (shopManager.Money < shopManager.Inventory[2, 1]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 2)
        {
            if (shopManager.Money < shopManager.Inventory[2, 2]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 3)
        {
            if (shopManager.Money < shopManager.Inventory[2, 3]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 4)
        {
            if (shopManager.Money < shopManager.Inventory[2, 4]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 5)
        {
            if (shopManager.Money < shopManager.Inventory[2, 5]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 6)
        {
            if (shopManager.Money < shopManager.Inventory[2, 6]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (shopManager.Inventory[3, 6] == 0)
        {
            SecretTier.SetActive(false);
        }
        if (shopManager.Inventory[3, 6] >= 1)
        {
            SecretTier.SetActive(true);
        }
        if (ItemID == 7)
        {
            if (shopManager.Money < shopManager.Inventory[2, 7]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 8)
        {
            if (shopManager.Money < shopManager.Inventory[2, 8]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 9)
        {
            if (shopManager.Money < shopManager.Inventory[2, 9]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 10)
        {
            if (shopManager.Money < shopManager.Inventory[2, 10]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        if (ItemID == 11)
        {
            if (shopManager.Money < shopManager.Inventory[2, 11]) ButtonComponent.interactable = false;
            else ButtonComponent.interactable = true;
        }
        /*
        S0.SetActive(shopManager.Inventory[3, 8] == 0);
        S1.SetActive(shopManager.Inventory[3, 8] == 1);
        S2.SetActive(shopManager.Inventory[3, 8] == 2);
        S3.SetActive(shopManager.Inventory[3, 8] >= 3);

        F0.SetActive(shopManager.Inventory[3, 9] == 0);
        F1.SetActive(shopManager.Inventory[3, 9] == 1);
        F2.SetActive(shopManager.Inventory[3, 9] == 2);
        F3.SetActive(shopManager.Inventory[3, 9] >= 3);

        I0.SetActive(shopManager.Inventory[3, 10] == 0);
        I1.SetActive(shopManager.Inventory[3, 10] == 1);
        I2.SetActive(shopManager.Inventory[3, 10] == 2);
        I3.SetActive(shopManager.Inventory[3, 10] >= 3);

        R0.SetActive(shopManager.Inventory[3, 11] == 0);
        R1.SetActive(shopManager.Inventory[3, 11] == 1);
        R2.SetActive(shopManager.Inventory[3, 11] == 2);
        R3.SetActive(shopManager.Inventory[3, 11] >= 3);
        */
        if (ItemID >= 8 && ItemID <= 11) //For upgrades
        {
            if (shopManager.Inventory[3, ItemID] >= 3)
            {
                //LevelText.text = "Upgrade Max";
                PriceText.text = ""; //Hide the price
                ButtonComponent.interactable = false;
            }
            else
            {
                //LevelText.text = "Upgrade: #" + shopManager.Inventory[3, ItemID].ToString();
                PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
                ButtonComponent.interactable = true;
            }
        }
        else if (ItemID >= 3 && ItemID <= 6) //Locking chicken tiers 3-6
        {
            int researchLevel = shopManager.Inventory[3, 11];

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
            shopManager.UpdateUI();
        }

        else //For normal shop items
        {
            PriceText.text = "Price: $" + shopManager.Inventory[2, ItemID].ToString();
            ButtonComponent.interactable = true;
        }
    }
}

