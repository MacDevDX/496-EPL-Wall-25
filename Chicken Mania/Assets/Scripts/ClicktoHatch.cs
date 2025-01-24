using UnityEngine;
using UnityEngine.InputSystem;

public class ClicktoHatch : MonoBehaviour
{
    InputSystem input;
    public float clicktoHatch, hatchCountdown;

    public GameObject chickObject;

    void Awake()
    {
        hatchCountdown = clicktoHatch;
        input = new InputSystem();
        input.Click.Touch.performed += OnTap;
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

    void OnTap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                hatchCountdown -= 1;
            }
        }
    }

    private void OnMouseDown()
    {
        //hatchCountdown -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (hatchCountdown == 0)
        {
            Destroy(gameObject);
            Instantiate(chickObject, transform.position, transform.rotation);
        }
    }
}
