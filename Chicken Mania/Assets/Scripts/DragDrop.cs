using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    private string dropZoneTag = "DropZone"; //Added for Drop to sell

    private Camera MainCamera;
    private WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 velocity = Vector3.zero;

    private Animator objectAnimator;  //Added to work with the animated objects

    private GameObject currentObject; //Track the object currently being dragged for selling
    public ShopManager shopManager; //Reference to ShopManager

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    private void OnEnable()
    {
        MouseClick.Enable();
        MouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        MouseClick.performed -= MousePressed;
        MouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Draggable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }
    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        objectAnimator = clickedObject.GetComponent<Animator>();  //Get the Animator component if it exists

        if (objectAnimator != null)
            objectAnimator.enabled = false; //Disable the animator while dragging

        float InitialDistance = Vector3.Distance(clickedObject.transform.position, MainCamera.transform.position);

        currentObject = clickedObject;  //Store object being dragged to sell

        while (MouseClick.ReadValue<float>() != 0) //If 1 it means holding click/button
        {
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
        UnityEngine.Debug.Log("Dragged object name: " + draggedObject.name);


        if (draggedObject.name.Contains("rhode"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 1 : 2;
        }
        else if (draggedObject.name.Contains("leghorn"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 2 : 3;
        }
        else if (draggedObject.name.Contains("astralorp"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 3 : 4;
        }
        else if (draggedObject.name.Contains("silkie"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 5 : 6;
        }
        else if (draggedObject.name.Contains("polish"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 4 : 5;
        }
        else if (draggedObject.name.Contains("easter"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 6 : 7;
        }
        else if (draggedObject.name.Contains("chicken"))
        {
            moneyEarned = draggedObject.name.Contains("egg") ? 4 : 50000;
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
