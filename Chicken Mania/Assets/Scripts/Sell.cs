using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;
using UnityEngine;

public class Sell : MonoBehaviour
{
    [SerializeField]
    private string dropZoneTag = "DropZone";
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
        transform.position += dragGesture.DeltaPosition;
        rb.MovePosition(transform.position + dragGesture.DeltaPosition);
    }

    private void OnDragEnd()
    {
        isDragging = false;
        //GetComponent<Collider>().enabled = true;
    }
    /*
    private void CheckForSellable()
    {
        Collider[] sellable = Physics.OverlapSphere(transform.position, 2f); // Adjust radius if needed
        List<GameObject> toSell = new List<GameObject>();

        foreach (Collider col in sellable)
        {
            if (col.CompareTag(sellableTag))
                toSell.Add(col.gameObject);
        }

        foreach (GameObject SellObject in toSell)
        {
            GiveMoney(SellObject);
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (!isDragging) return;
        if (other.CompareTag(sellableTag))
        {
            GiveMoney(other.gameObject);
            Destroy(other.gameObject);

        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (!isDragging) return;
        if (other.CompareTag(sellableTag))
        {
            GiveMoney(other.gameObject);
            Destroy(other.gameObject);
        }
    }
    */
    public void GiveMoney(GameObject droppedObject)
    {
        int moneyEarned = 0;

        if (droppedObject.name.Contains("rhode"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.25f) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.5f) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 1] * 0.7f) + ((shopManager.Inventory[2, 1] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("leghorn"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.25f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.5f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 2] * 0.7f) + ((shopManager.Inventory[2, 2] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("astralorp"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.25f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.5f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 3] * 0.7f) + ((shopManager.Inventory[2, 3] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("easter"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.25f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.5f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 4] * 0.7f) + ((shopManager.Inventory[2, 4] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("silkie"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.25f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.5f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 5] * 0.7f) + ((shopManager.Inventory[2, 5] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("polish"))
        {
            if (droppedObject.name.Contains("egg"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.25f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellEgg();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else if (droppedObject.name.Contains("chicks"))
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.5f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChick();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
            else
            {
                moneyEarned = (int)((shopManager.Inventory[2, 6] * 0.7f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
                shopManager.SellChicken();
                ShowMoneyIndicator(moneyEarned);
                Destroy(droppedObject);
            }
        }
        else if (droppedObject.name.Contains("bucket"))
        {
            moneyEarned = (int)((shopManager.Inventory[2, 7] * 0.7f) + ((shopManager.Inventory[2, 6] * (shopManager.Inventory[3, 9] * 0.1f))));
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
            shopManager.Money_Text.text = shopManager.Money.ToString();
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

        Destroy(indicator, .5f);
    }

}
