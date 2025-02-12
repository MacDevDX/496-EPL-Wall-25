/*using System.Collections;
using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class DragDrop : MonoBehaviour
{
    [SerializeField]
    private InputAction MouseClick;
    [SerializeField]
    private float MouseDragPhysicsSpeed = 150;
    [SerializeField]
    private float MouseDragSpeed = .1f;

    [SerializeField]
    private string dropZoneTag = "DropZone";

    private Camera MainCamera;
    private WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 velocity = Vector3.zero;

    private Animator objectAnimator;
    private GameObject currentObject;

    public ShopManager shopManager;

    private TransformGesture dragGesture;
    private TapGesture tapGesture;

    private Rigidbody objectRigidbody;
    private bool isDragging = false;

    private void Awake()
    {
        MainCamera = Camera.main;

        // Add gesture components dynamically
        dragGesture = gameObject.AddComponent<TransformGesture>();
        tapGesture = gameObject.AddComponent<TapGesture>();

        // Register event listeners
        dragGesture.Transformed += OnDrag;
        tapGesture.Tapped += OnTap;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }
        if (Input.GetMouseButton(0) && currentObject != null)
        {
            OnMouseDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void OnMouseClick()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Draggable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable")))
            {
                currentObject = hit.collider.gameObject;
                objectAnimator = currentObject.GetComponent<Animator>();
                objectRigidbody = currentObject.GetComponent<Rigidbody>();

                if (objectAnimator != null)
                    objectAnimator.enabled = false;

                if (objectRigidbody != null)
                    objectRigidbody.isKinematic = true;  // Disable physics while dragging
                /*
                if (objectRigidbody != null)
                {
                    objectRigidbody.isKinematic = true;  // Disable physics while dragging
                }
                if (currentObject.TryGetComponent(out NavMeshAgent agent))
                {
                    agent.enabled = false;  // Disable navigation
                }
                */
                isDragging = true;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (currentObject == null || objectRigidbody == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.WorldToScreenPoint(currentObject.transform.position).z;
        Vector3 worldPos = MainCamera.ScreenToWorldPoint(mousePos);

        // Move object using Rigidbody to prevent collision issues
        objectRigidbody.MovePosition(Vector3.Lerp(objectRigidbody.position, worldPos, MouseDragSpeed));
    }
    private void EndDrag()
    {
        if (currentObject != null)
        {
            if (objectRigidbody != null)
                objectRigidbody.isKinematic = false;  // Restore physics

            CheckDropZone();

            if (objectAnimator != null)
                objectAnimator.enabled = true;
            /*
            if (currentObject.TryGetComponent(out NavMeshAgent agent))
            {
                agent.enabled = true;  // Re-enable navigation
            }
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = false;  // Re-enable physics
            }
            */
            currentObject = null;
            objectRigidbody = null;
            isDragging = false;
        }
    }

    private void OnTap(object sender, System.EventArgs e)
    {
        OnMouseClick();
    }

    private void OnDrag(object sender, System.EventArgs e)
    {
        if (currentObject == null) return;

        // Get TouchScript transformation data
        Vector3 newPosition = dragGesture.ScreenPosition;

        // Convert touch position to world position
        newPosition.z = MainCamera.WorldToScreenPoint(currentObject.transform.position).z;
        Vector3 worldPos = MainCamera.ScreenToWorldPoint(newPosition);

        // Apply smooth drag effect
        currentObject.transform.position = Vector3.SmoothDamp(currentObject.transform.position, worldPos, ref velocity, MouseDragSpeed);
    }

    private void CheckDropZone()
    {
        if (currentObject != null)
        {
            Collider[] hitColliders = Physics.OverlapBox(currentObject.transform.position, currentObject.GetComponent<Collider>().bounds.extents, Quaternion.identity);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(dropZoneTag))
                {
                    GiveMoney(currentObject);
                    Destroy(currentObject);
                    break;
                }
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
*/

