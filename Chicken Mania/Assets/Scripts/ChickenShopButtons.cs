using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChickenShopButtons : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceText;
    public GameObject ShopManager;

    void Update()
    {
        PriceText.text = "Price: $" + ShopManager.GetComponent<ChickenShop>().Chicken_Tiers[2, ItemID].ToString();
    }

}
