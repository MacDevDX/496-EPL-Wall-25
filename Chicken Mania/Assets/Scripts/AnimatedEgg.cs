using UnityEngine;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;

public class AnimatedEgg : MonoBehaviour
{
    private Animator eggAnimator;
    private Rigidbody rb;
    private TransformGesture dragGesture;
    private int tap = 0; // imported from egg hatch script tbd
    private ShopManager shopManager;
    //private bool isDragging = false;
    private GameObject currentDropZone = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        float randomY = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(0, randomY, 0);
        eggAnimator = GetComponent<Animator>();
        eggAnimator.SetTrigger("idle");

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;  // Prevent unwanted movement
        }

        dragGesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();
        dragGesture.Transformed += OnDrag;
        dragGesture.TransformCompleted += OnDragEnd; // Detect drag release
    }

    // Update is called once per frame
    void Update()
    {
        if (tap>=1)
        {
            eggAnimator.SetTrigger("tap");
        }
    }

    private void OnDrag(object sender, System.EventArgs e)
    {
        if (shopManager.dragZone && shopManager.dragZone.activeSelf)
        {
            rb.isKinematic = true; // Ensure no physics interference
            Vector3 newPosition = transform.position + dragGesture.DeltaPosition * 2.8f;
            rb.MovePosition(newPosition); // Move with physics
            //isDragging = true;
        }
    }

    private void OnDragEnd(object sender, System.EventArgs e)
    {
        //isDragging = false;
        rb.isKinematic = false; // Reactivate physics

        // If released over DropZone, sell the egg
        if (currentDropZone != null)
        {
            SellEgg();
        }
    }

    private void SellEgg()
    {
        Sell sellScript = currentDropZone.GetComponent<Sell>();
        if (sellScript != null)
        {
            sellScript.GiveMoney(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            currentDropZone = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DropZone"))
        {
            currentDropZone = null;
        }
    }
}
