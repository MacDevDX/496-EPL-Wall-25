using UnityEngine;
using TMPro;
using TouchScript.Gestures;

public class InactivityHandler : MonoBehaviour
{
    public float inactivityThreshold = 60f; // Seconds before warning
    public float returnToMenuTime = 30f;    // Seconds before message disappears
    private float lastInteractionTime;
    private float countdownTime;
    private bool countdownStarted = false;

    public GameObject inactivityWarning;
    public TextMeshProUGUI countdownText;
    // public GameObject hudObject; // Reference to the HUD object
    public GameObject screenTouchArea; // Reference to the screen Section
    public GameObject inactivityTouchArea; // Reference to the screen touch area for inactivity
    public ScreenController screenController; // Reference to screen controller script
    public ShopManager shopManager;


    private void Start()
    {
        lastInteractionTime = Time.time;

        if (inactivityWarning != null)
        {
            inactivityWarning.SetActive(false);
        }

        RegisterTouchGestures();
    }

    private void Update()
    {
        // Check inactivity per instance
        if (!countdownStarted && Time.time - lastInteractionTime > inactivityThreshold)
        {
            ShowInactivityWarning();
        }

        // Handle countdown per instance
        if (countdownStarted)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text = "Returning to Main Menu in: " + Mathf.Ceil(countdownTime) + "s";

            if (countdownTime <= 0)
            {
         
                ReturnToTitle();
                
            }
        }
    }

    private void RegisterTouchGestures()
    {
        if (screenTouchArea != null)
        {
            TapGesture tapGesture = screenTouchArea.GetComponent<TapGesture>();
            PressGesture pressGesture = screenTouchArea.GetComponent<PressGesture>();

            if (tapGesture != null)
            {
                tapGesture.Tapped += OnUserInteraction;
            }

            if (pressGesture != null)
            {
                pressGesture.Pressed += OnUserInteraction;
            }

            
        }

        if (inactivityTouchArea != null)
        {
            TapGesture inactivityTapGesture = inactivityTouchArea.GetComponent<TapGesture>();

            if (inactivityTapGesture != null)
            {
                Debug.Log($"Inactive Area is pressed in {screenTouchArea}");
                inactivityTapGesture.Tapped += OnUserInteraction; 
            }
        }
    }

    private void OnUserInteraction(object sender, System.EventArgs e)
    {
        Debug.Log($"ScreenArea is pressed in {screenTouchArea}");
        ResetInactivityTimer();

    }

    private void ResetInactivityTimer()
    {
        lastInteractionTime = Time.time;

        if (inactivityWarning != null && inactivityWarning.activeSelf)
        {
            inactivityWarning.SetActive(false);
            countdownStarted = false;
        }
    }

    private void ShowInactivityWarning()
    {
        if (inactivityWarning != null && !inactivityWarning.activeSelf)
        {
            inactivityWarning.SetActive(true);
            countdownStarted = true;
            countdownTime = returnToMenuTime;
        }
    }

    private void ReturnToTitle()
    {

        if (inactivityWarning != null)
        {
            inactivityWarning.SetActive(false);
        }

        countdownStarted = false;
        lastInteractionTime = Time.time;

        if (screenController != null)
        {
            Debug.Log("Return to Start Page");
            screenController.ReturnToTitlePage();
            shopManager.ResetGame();
        }
        else
        {
            Debug.Log("Screen not found" + gameObject.name);
        }


    }
}
