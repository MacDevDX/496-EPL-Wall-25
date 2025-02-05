using UnityEngine;
using UnityEngine.InputSystem;
using TouchScript.Gestures;

public class ClicktoHatch : MonoBehaviour
{
    InputSystem input;
    public float clicktoHatch, hatchCountdown;

    public GameObject chickObject;

    private ShopManager shopManager; //private so don't need to attach in inspector but will need call in Awake


    void Awake()
    {
        hatchCountdown = clicktoHatch;
        input = new InputSystem();
        input.Click.Touch.performed += OnTap;

        shopManager = Object.FindFirstObjectByType<ShopManager>();

        //TouchScript Gesture
        TapGesture tapGesture = gameObject.AddComponent<TapGesture>();
        tapGesture.Tapped += OnTouchTap;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Click.Touch.performed += OnTap;

    }

    private void OnDisable()
    {
        input.Disable();
        input.Click.Touch.performed -= OnTap;
    }
    //For normal input systems
    void OnTap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    hatchCountdown -= 1;
                }
            }
        }
    }
    //For TouchScript system
    private void OnTouchTap(object sender, System.EventArgs e)
    {
        hatchCountdown -= 1;
    }

    private void OnMouseDown()
    {
        //hatchCountdown -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (hatchCountdown <= 0)  //in case lag and goes negative
        {
            Destroy(gameObject);
            Instantiate(chickObject, transform.position, transform.rotation);
            shopManager.HatchEgg();
        }
    }
}
