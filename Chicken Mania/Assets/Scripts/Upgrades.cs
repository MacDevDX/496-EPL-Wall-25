using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Upgrades : MonoBehaviour
{
    public Dictionary<int, UpgradeTier[]> UpgradeCategories = new Dictionary<int, UpgradeTier[]>();
    public int Money;
    public TextMeshProUGUI Money_Text;

    public List<UpgradesButtons> UpgradeButtons;  // List of all buttons (drag these from the inspector)


    void Start()
    {
        Money_Text.text = Money.ToString();

        //3 tiers for a Supplement upgrade (ID: 1)
        UpgradeCategories[1] = new UpgradeTier[]
        {
            new UpgradeTier(1, 50, 1),  // Tier 1: Shop ID 1, Price 50, Level 1
            new UpgradeTier(1, 100, 2), // Tier 2
            new UpgradeTier(1, 200, 3)  // Tier 3
        };

        //3 tiers for Feed upgrade category (ID: 2)
        UpgradeCategories[2] = new UpgradeTier[]
        {
            new UpgradeTier(2, 75, 1),  // Tier 1
            new UpgradeTier(2, 150, 2), // Tier 2
            new UpgradeTier(2, 300, 3)  // Tier 3
        };

        //3 tiers for Incubator upgrade category (ID: 3)
        UpgradeCategories[3] = new UpgradeTier[]
        {
            new UpgradeTier(3, 75, 1),  // Tier 1
            new UpgradeTier(3, 150, 2), // Tier 2
            new UpgradeTier(3, 300, 3)  // Tier 3
        };

        //3 tiers for Research upgrade category (ID: 4)
        UpgradeCategories[4] = new UpgradeTier[]
        {
            new UpgradeTier(4, 75, 1),  // Tier 1
            new UpgradeTier(4, 150, 2), // Tier 2
            new UpgradeTier(4, 300, 3)  // Tier 3
        };
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int categoryID = ButtonRef.GetComponent<UpgradesButtons>().ItemID; // Upgrade category ID
        int tierIndex = ButtonRef.GetComponent<UpgradesButtons>().TierIndex; // Tier index (0, 1, 2)

        // Check if the category exists and the tier index is valid
        if (UpgradeCategories.ContainsKey(categoryID))
        {
            if (tierIndex < UpgradeCategories[categoryID].Length)
            {
                UpgradeTier tier = UpgradeCategories[categoryID][tierIndex];

                if (Money >= tier.Price)
                {
                    Money -= tier.Price;

                    // Update the UI
                    Money_Text.text = Money.ToString();
                    ButtonRef.GetComponent<UpgradesButtons>().LevelText.text = "Level: "+tier.Level.ToString();

                    ButtonRef.GetComponent<UpgradesButtons>().UpdateButton(); // Refresh button with new info

                    // Move to the next tier if possible
                    if (tierIndex < UpgradeCategories[categoryID].Length - 1)
                    {
                        ButtonRef.GetComponent<UpgradesButtons>().TierIndex++;  // Update tier index for next tier
                    }
                    else
                    {
                        ButtonRef.GetComponent<Button>().interactable = false; // Disable button if max tier is reached
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Category ID " + categoryID + " not found in dictionary.");
        }
    }
}
