using System.Collections;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
using System.Diagnostics;
using System.Runtime.CompilerServices;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.InputSystem;

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

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private Animator objectAnimator;  //Added to work with the animated objects

    private GameObject currentObject; //Track the object currently being dragged for selling
    public ShopManager shopManager; //Reference to ShopManager
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    private Animator objectAnimator;
    private GameObject currentObject;

    public ShopManager shopManager;

    private TransformGesture dragGesture;
    private TapGesture tapGesture;
>>>>>>> Stashed changes

    private Rigidbody objectRigidbody;
    private bool isDragging = false;

    private Rigidbody objectRigidbody;
    private bool isDragging = false;

    private Rigidbody objectRigidbody;
    private bool isDragging = false;

    private void Awake()
    {
        MainCamera = Camera.main;
<<<<<<< Updated upstream
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private void OnEnable()
    {
        MouseClick.Enable();
        MouseClick.performed += MousePressed;
=======

=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        // Add gesture components dynamically
        dragGesture = gameObject.AddComponent<TransformGesture>();
        tapGesture = gameObject.AddComponent<TapGesture>();

        // Register event listeners
        dragGesture.Transformed += OnDrag;
        tapGesture.Tapped += OnTap;
    }
<<<<<<< Updated upstream
<<<<<<< Updated upstream

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
=======

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
=======

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
>>>>>>> Stashed changes
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
>>>>>>> Stashed changes
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
>>>>>>> Stashed changes
    }

    private void OnMouseClick()
    {
<<<<<<< Updated upstream
        MouseClick.performed -= MousePressed;
        MouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
=======
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
>>>>>>> Stashed changes
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Draggable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
<<<<<<< Updated upstream
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }
    private IEnumerator DragUpdate(GameObject clickedObject)
=======
                currentObject = hit.collider.gameObject;
                objectAnimator = currentObject.GetComponent<Animator>();
                objectRigidbody = currentObject.GetComponent<Rigidbody>();

                if (objectAnimator != null)
                    objectAnimator.enabled = false;

                if (objectRigidbody != null)
                    objectRigidbody.isKinematic = true;  // Disable physics while dragging

                isDragging = true;
            }
        }
    }

    private void OnMouseDrag()
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
    {
        if (currentObject == null || objectRigidbody == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.WorldToScreenPoint(currentObject.transform.position).z;
        Vector3 worldPos = MainCamera.ScreenToWorldPoint(mousePos);

        // Move object using Rigidbody to prevent collision issues
        objectRigidbody.MovePosition(Vector3.Lerp(objectRigidbody.position, worldPos, MouseDragSpeed));
    }
    private void EndDrag()
>>>>>>> Stashed changes
    {
        if (currentObject == null || objectRigidbody == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.WorldToScreenPoint(currentObject.transform.position).z;
        Vector3 worldPos = MainCamera.ScreenToWorldPoint(mousePos);

        // Move object using Rigidbody to prevent collision issues
        objectRigidbody.MovePosition(Vector3.Lerp(objectRigidbody.position, worldPos, MouseDragSpeed));
    }
    private void EndDrag()
>>>>>>> Stashed changes
    {
        if (currentObject == null || objectRigidbody == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = MainCamera.WorldToScreenPoint(currentObject.transform.position).z;
        Vector3 worldPos = MainCamera.ScreenToWorldPoint(mousePos);

        // Move object using Rigidbody to prevent collision issues
        objectRigidbody.MovePosition(Vector3.Lerp(objectRigidbody.position, worldPos, MouseDragSpeed));
    }
    private void EndDrag()
>>>>>>> Stashed changes
    {
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        objectAnimator = clickedObject.GetComponent<Animator>();  //Get the Animator component if it exists

        if (objectAnimator != null)
            objectAnimator.enabled = false; //Disable the animator while dragging

        float InitialDistance = Vector3.Distance(clickedObject.transform.position, MainCamera.transform.position);

        currentObject = clickedObject;  //Store object being dragged to sell

        while (MouseClick.ReadValue<float>() != 0) //If 1 it means holding click/button
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            if (clickedObject == null)
            {
                yield break;
            }

            Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(InitialDistance) - clickedObject.transform.position;
                rb.linearVelocity = direction * MouseDragPhysicsSpeed;
                yield return WaitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(InitialDistance), ref velocity, MouseDragSpeed);
                yield return null;
            }
        }

        CheckDropZone(); //Call function to check if object is in sell zone

        if (objectAnimator != null)
            objectAnimator.enabled = true; //Re-enable the animator when dragging is finished
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            if (objectRigidbody != null)
                objectRigidbody.isKinematic = false;  // Restore physics

            CheckDropZone();

            if (objectAnimator != null)
                objectAnimator.enabled = true;

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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======

        // Apply smooth drag effect
        currentObject.transform.position = Vector3.SmoothDamp(currentObject.transform.position, worldPos, ref velocity, MouseDragSpeed);
    }
>>>>>>> Stashed changes

        // Apply smooth drag effect
        currentObject.transform.position = Vector3.SmoothDamp(currentObject.transform.position, worldPos, ref velocity, MouseDragSpeed);
    }
>>>>>>> Stashed changes

        // Apply smooth drag effect
        currentObject.transform.position = Vector3.SmoothDamp(currentObject.transform.position, worldPos, ref velocity, MouseDragSpeed);
    }
>>>>>>> Stashed changes

    }
    /************************ Below is the selling functions ******************************/
    private void CheckDropZone()
    {
        if (currentObject != null)
        {
            //Check if the object is in the drop zone (using a trigger collision)
            Collider[] hitColliders = Physics.OverlapBox(currentObject.transform.position, currentObject.GetComponent<Collider>().bounds.extents, Quaternion.identity);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(dropZoneTag))  //Check if the object in zone
                {
                    GiveMoney(currentObject); //Give money based on object
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
                moneyEarned = 1;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 2;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("leghorn"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 2;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 3;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("astralorp"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 3;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 4;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("silkie"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 5;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 6;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("polish"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 4;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 5;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("easter"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 6;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 7;
                shopManager.SellChicken();
            }
        }
        else if (draggedObject.name.Contains("chicken"))
        {
            if (draggedObject.name.Contains("egg"))
            {
                moneyEarned = 6;
                shopManager.SellEgg();
            }
            else
            {
                moneyEarned = 50000;
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
