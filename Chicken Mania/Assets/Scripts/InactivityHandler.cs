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

    public GameObject inactivityWarningGreen;
    public GameObject inactivityWarningOrange;
    public TextMeshProUGUI greenCountdownText;
    public TextMeshProUGUI orangeCountdownText;

    public ShopManager shopManager;
    public GameObject hudObject; // Reference to the HUD object
    public GameObject Screen;

    private void Start()
    {
        lastInteractionTime = Time.time;

        if (inactivityWarningGreen != null)
        {
            inactivityWarningGreen.SetActive(false);
        }
        if (inactivityWarningOrange != null)
        {
            inactivityWarningOrange.SetActive(false);
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
            string countdownMessage = Mathf.Ceil(countdownTime) + "s";

            if (inactivityWarningGreen != null && inactivityWarningGreen.activeSelf && greenCountdownText != null)
            {
                greenCountdownText.text = countdownMessage;
            }

            if (inactivityWarningOrange != null && inactivityWarningOrange.activeSelf && orangeCountdownText != null)
            {
                orangeCountdownText.text = countdownMessage;
            }

            if (countdownTime <= 0)
            {
                if (inactivityWarningGreen != null)
                {
                    inactivityWarningGreen.SetActive(false);
                }

                if (inactivityWarningOrange != null)
                {
                    inactivityWarningOrange.SetActive(false);
                }

                countdownStarted = false; // Reset so it can trigger again later
                shopManager.ResetGame();
            }
        }
    }

    private void RegisterTouchGestures()
    {
        if (Screen != null)
        {
            TapGesture tapGesture = Screen.GetComponent<TapGesture>();
            PressGesture pressGesture = Screen.GetComponent<PressGesture>();

            if (tapGesture != null)
            {
                tapGesture.Tapped += OnUserInteraction;
            }

            if (pressGesture != null)
            {
                pressGesture.Pressed += OnUserInteraction;
            }
        }

        if (hudObject != null)
        {
            TapGesture tapGesture = hudObject.GetComponent<TapGesture>();
            PressGesture pressGesture = hudObject.GetComponent<PressGesture>();

            if (tapGesture != null)
            {
                tapGesture.Tapped += OnUserInteraction;
            }

            if (pressGesture != null)
            {
                pressGesture.Pressed += OnUserInteraction;
            }
        }

    }

    private void OnUserInteraction(object sender, System.EventArgs e)
    {
        ResetInactivityTimer();
    }

    public void ResetInactivityTimer()
    {
        lastInteractionTime = Time.time;

        if (inactivityWarningGreen != null)
        {
            inactivityWarningGreen.SetActive(false);
        }

        if (inactivityWarningOrange != null)
        {
            inactivityWarningOrange.SetActive(false);
        }

        countdownStarted = false;
    }

    private void ShowInactivityWarning()
    {
        if (!countdownStarted)
        {
            bool showGreen = Random.value < 0.5f;

            if (showGreen && inactivityWarningGreen != null)
            {
                inactivityWarningGreen.SetActive(true);

                if (inactivityWarningOrange != null)
                {
                    inactivityWarningOrange.SetActive(false);
                }
            }
            else if (!showGreen && inactivityWarningOrange != null)
            {
                inactivityWarningOrange.SetActive(true);

                if (inactivityWarningGreen != null)
                {
                    inactivityWarningGreen.SetActive(false);
                }
            }

            countdownStarted = true;
            countdownTime = returnToMenuTime;
        }
    }

}
