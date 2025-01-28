using TMPro;
using UnityEngine;


public class UpgradesButtons : MonoBehaviour
{
    public int ItemID;    //Upgrade category (supplements, feed, incu, r&d)
    public int TierIndex; //The specific tier within the category (lvl 1,2,3)
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI LevelText;
    public GameObject ShopManager;

    public void UpdateButton()
    {
        // Get the current upgrade tier details
        UpgradeTier tier = ShopManager.GetComponent<Upgrades>().UpgradeCategories[ItemID][TierIndex];

        // Update the price and quantity text
        PriceText.text = "Price: $" + tier.Price.ToString();
        LevelText.text = "Upgrade Level:" + tier.Level.ToString();

    }

    // This function can be called when the player purchases an upgrade
    public void OnUpgradePurchased()
    {
        // Update the TierIndex to the next tier
        TierIndex++;

        // After upgrade, update button text
        UpdateButton();
    }
}
