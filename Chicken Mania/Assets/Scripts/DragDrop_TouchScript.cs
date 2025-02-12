using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class DragDrop_TouchScript : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;
    private Animator objectAnimator;
    private GameObject currentObject;
    private Rigidbody objectRigidbody;
    private bool isDragging = false;
    private TransformGesture dragGesture;
    public ShopManager shopManager;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        // Find all draggable objects when DropZone is active
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject obj in draggableObjects)
        {
            // Ensure each object has a TransformGesture
            dragGesture = obj.GetComponent<TransformGesture>() ?? obj.AddComponent<TransformGesture>();

            // Add event listener for dragging
            dragGesture.Transformed += OnDrag;
        }
    }

    private void OnDisable()
    {
        // Remove event listeners to prevent memory leaks
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject obj in draggableObjects)
        {
            TransformGesture gesture = obj.GetComponent<TransformGesture>();
            if (gesture != null) gesture.Transformed -= OnDrag;
        }
    }

    private void OnDrag(object sender, System.EventArgs e)
    {
        TransformGesture gesture = sender as TransformGesture;
        if (gesture == null || !isDragging) return;

        GameObject draggedObject = gesture.gameObject;

        // Get touch delta position
        Vector3 delta = gesture.DeltaPosition;

        // Convert touch movement to world space
        Vector3 worldDelta = mainCamera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 0));

        // Smooth movement
        draggedObject.transform.position = Vector3.SmoothDamp(
            draggedObject.transform.position,
            draggedObject.transform.position + worldDelta,
            ref velocity,
            0.1f
        );
    }

    private void StartDragging(GameObject obj)
    {
        currentObject = obj;
        objectAnimator = currentObject.GetComponent<Animator>();
        objectRigidbody = currentObject.GetComponent<Rigidbody>();

        if (objectAnimator != null) objectAnimator.enabled = false;
        if (objectRigidbody != null) objectRigidbody.isKinematic = true;
        isDragging = true;
    }

    private void StopDragging()
    {
        if (currentObject == null) return;

        if (objectRigidbody != null) objectRigidbody.isKinematic = false;
        if (objectAnimator != null) objectAnimator.enabled = true;

        CheckDropZone();
        currentObject = null;
        objectRigidbody = null;
        isDragging = false;
    }

    private void CheckDropZone()
    {
        if (currentObject == null) return;

        Collider[] hitColliders = Physics.OverlapBox(
            currentObject.transform.position,
            currentObject.GetComponent<Collider>().bounds.extents,
            Quaternion.identity
        );

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("DropZone"))
            {
                GiveMoney(currentObject);
                Destroy(currentObject);
                break;
            }
        }
    }

    private void GiveMoney(GameObject draggedObject)
    {
        int moneyEarned = 0;
        //UnityEngine.Debug.Log("Dragged object name: " + draggedObject.name);

        if (draggedObject.name.Contains("rhode"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 2 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("leghorn"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 2 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 3 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("astralorp"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 3 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("silkie"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("polish"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 4 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 5 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("easter"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 7 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("chicken"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 6 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else if (draggedObject.name.Contains("chicks"))
            {
                moneyEarned = 1 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 50000 + 1 * shopManager.Inventory[3, 8];
                shopManager.SellChicken();
            }
        }

        if (moneyEarned > 0)
        {
            shopManager.Money += moneyEarned;
            shopManager.Money_Text.text = shopManager.Money.ToString();
        }
        else
        {
            return;
        }
    }
}


