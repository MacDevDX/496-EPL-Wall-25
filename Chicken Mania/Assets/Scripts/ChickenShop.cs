using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChickenShop : MonoBehaviour
{
    public int[,] Chicken_Tiers = new int[3,7]; //Array for tier of chickens (avoid using index 0)
    public int Money;
    public TextMeshProUGUI Money_Text;

    void Start()
    {
        Money_Text.text = Money.ToString();

        //Shop ID
        Chicken_Tiers[1, 1] = 1;
        Chicken_Tiers[1, 2] = 2;
        Chicken_Tiers[1, 3] = 3;
        Chicken_Tiers[1, 4] = 4;
        Chicken_Tiers[1, 5] = 5;
        Chicken_Tiers[1, 6] = 6;

        //Price
        Chicken_Tiers[2, 1] = 50;
        Chicken_Tiers[2, 2] = 100;
        Chicken_Tiers[2, 3] = 200;
        Chicken_Tiers[2, 4] = 400;
        Chicken_Tiers[2, 5] = 600;
        Chicken_Tiers[2, 6] = 1000;

    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (Money >= Chicken_Tiers[2, ButtonRef.GetComponent<ChickenShopButtons>().ItemID])
        {
            Money -= Chicken_Tiers[2, ButtonRef.GetComponent<ChickenShopButtons>().ItemID];  //Updates total money
            Money_Text.text = Money.ToString();
        }
    }
}
