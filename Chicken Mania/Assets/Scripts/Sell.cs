using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;
using UnityEngine;

public class Sell : MonoBehaviour
{
    [SerializeField]
    private string sellableTag = "Draggable";

    public ShopManager shopManager;

    private TransformGesture dragGesture;
    private Rigidbody rb;
    private bool isDragging = false;
    public GameObject moneyIndicator;

    void Start()
    {
        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();

        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;
        dragGesture.TransformCompleted += (s, e) => OnDragEnd();
    }
    private void OnDrag(object sender, System.EventArgs e)
    {
        isDragging = true;
        //GetComponent<Collider>().enabled = false;
        shopManager.ResetInactivityTimer();

        transform.position += dragGesture.DeltaPosition;
        rb.MovePosition(transform.position + dragGesture.DeltaPosition);
    }

    private void OnDragEnd()
    {
        isDragging = false;
        //GetComponent<Collider>().enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isDragging) return;
        if (other.CompareTag(sellableTag))
        {
            GiveMoney(other.gameObject);
            //Destroy(other.gameObject);

        }
    }


    public void GiveMoney(GameObject droppedObject)
    {
        int moneyEarned = 0;

        if (droppedObject.name.Contains("rhode"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * shopManager.EggValue) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * shopManager.ChickValue) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("leghorn"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * shopManager.EggValue) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * shopManager.ChickValue) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("astralorp"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * shopManager.EggValue) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * shopManager.ChickValue) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("easter"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * shopManager.EggValue) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * shopManager.ChickValue) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("silkie"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * shopManager.EggValue) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * shopManager.ChickValue) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("polish"))
        {
            if (droppedObject.name.Contains("eggs"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * shopManager.EggValue) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * shopManager.ChickValue) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicken") && (!shopManager.TycoonMode))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * shopManager.ChickenValue) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("bucket") && (!shopManager.TycoonMode))
        {
            moneyEarned = (int)((shopManager.Inventory[2, 7] * 0.6f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
            shopManager.SellChicken();
            ShowMoneyIndicator(moneyEarned);
            Destroy(droppedObject);
        }
        else if (droppedObject.name.Contains("drumstick"))
        {
            moneyEarned = (int)((shopManager.Inventory[2, 7] * 0.1f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
            shopManager.SellChicken();
            ShowMoneyIndicator(moneyEarned);
            Destroy(droppedObject);
        }
        else if (droppedObject.name.Contains("golden"))
        {
            moneyEarned = Mathf.Max(1, Mathf.FloorToInt(shopManager.Money * 0.1f)); shopManager.Money += moneyEarned;
            shopManager.SellEgg();
            ShowMoneyIndicator(moneyEarned);
            Destroy(droppedObject);
        }

        if (moneyEarned > 0)
        {
            shopManager.Money += moneyEarned;
            shopManager.UpdateUI();
            //shopManager.Money_Text.text = "$" + shopManager.Money.ToString();
        }
    }

    private void ShowMoneyIndicator(int moneyEarned)
    {
        GameObject indicator = Instantiate(moneyIndicator, transform.position + Vector3.up * .8f, Quaternion.Euler(45, 0, 0));
        indicator.transform.SetParent(transform);
        TMPro.TMP_Text textComponent = indicator.GetComponentInChildren<TMPro.TMP_Text>();
        textComponent.text = $"+{moneyEarned}";
        textComponent.color = Color.green;
        textComponent.fontSize *= 2;

        Destroy(indicator, 1f);
    }

}
