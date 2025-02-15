using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TouchScript.Gestures;
using TouchScript;
using TouchScript.Gestures.TransformGestures;

public class InactivityHandler : MonoBehaviour
{
    public float inactivityThreshold = 60f; // Time in seconds before inactivity warning
    public float returnToMenuTime = 30f; // Time before returning to the main menu
    private float lastInteractionTime;
    private float countdownTime;
    private bool countdownStarted = false;

    public GameObject inactivityWarning;
    public TextMeshProUGUI countdownText;

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
        // Start inactivity warning if threshold is exceeded
        if (!countdownStarted && Time.time - lastInteractionTime > inactivityThreshold)
        {
            ShowInactivityWarning();
        }

        // Handle countdown to return to main menu
        if (countdownStarted)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text = "Returning to Main Menu in: " + Mathf.Ceil(countdownTime) + "s";

            if (countdownTime <= 0)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    private void RegisterTouchGestures()
    {
        // Find all touch gestures
        TapGesture[] tapGestures = FindObjectsOfType<TapGesture>();
        PressGesture[] pressGestures = FindObjectsOfType<PressGesture>();
        ReleaseGesture[] releaseGestures = FindObjectsOfType<ReleaseGesture>();
        FlickGesture[] flickGestures = FindObjectsOfType<FlickGesture>();
        TransformGesture[] transformGestures = FindObjectsOfType<TransformGesture>();

        // Subscribe to all touch events
        foreach (TapGesture tap in tapGestures) tap.Tapped += OnUserInteraction;
        foreach (PressGesture press in pressGestures) press.Pressed += OnUserInteraction;
        foreach (ReleaseGesture release in releaseGestures) release.Released += OnUserInteraction;
        foreach (FlickGesture flick in flickGestures) flick.Flicked += OnUserInteraction;
        foreach (TransformGesture transform in transformGestures) transform.Transformed += OnUserInteraction;
    }

    private void OnUserInteraction(object sender, System.EventArgs e)
    {
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

    private void OnDestroy()
    {
        // Unsubscribe from gestures to prevent memory leaks
        TapGesture[] tapGestures = FindObjectsOfType<TapGesture>();
        PressGesture[] pressGestures = FindObjectsOfType<PressGesture>();
        ReleaseGesture[] releaseGestures = FindObjectsOfType<ReleaseGesture>();
        FlickGesture[] flickGestures = FindObjectsOfType<FlickGesture>();
        TransformGesture[] transformGestures = FindObjectsOfType<TransformGesture>();

        foreach (TapGesture tap in tapGestures) tap.Tapped -= OnUserInteraction;
        foreach (PressGesture press in pressGestures) press.Pressed -= OnUserInteraction;
        foreach (ReleaseGesture release in releaseGestures) release.Released -= OnUserInteraction;
        foreach (FlickGesture flick in flickGestures) flick.Flicked -= OnUserInteraction;
        foreach (TransformGesture transform in transformGestures) transform.Transformed -= OnUserInteraction;
    }
}
