using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TouchScript.Gestures;

public class InactivityHandler : MonoBehaviour
{
    public float inactivityThreshold = 60f; //seconds
    public float returnToMenuTime = 30f;
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
        //Check for mouse clicks
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            ResetInactivityTimer();
        }

        //Inactivity timer
        if (!countdownStarted && Time.time - lastInteractionTime > inactivityThreshold)
        {
            ShowInactivityWarning();
        }

        //Goes to main menu after idle too long
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
        TapGesture[] tapGestures = FindObjectsByType<TapGesture>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        PressGesture[] pressGestures = FindObjectsByType<PressGesture>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (TapGesture tapGesture in tapGestures)
        {
            tapGesture.Tapped += OnUserInteraction;
        }

        foreach (PressGesture pressGesture in pressGestures)
        {
            pressGesture.Pressed += OnUserInteraction;
        }
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
}
