using UnityEngine;
using TouchScript.Gestures;

public class ScreenController : MonoBehaviour
{
    public GameObject startUI;  // Assign Canvas StartUI in the Inspector
    public GameObject gameUI;   // Assign Canvas GameUI in the Inspector
    public GameObject gameObjects; // Assign GameObjects in the Inspector
    public GameObject shopManager; // Assign ShopManager in the Inspector
    public string screenName;   // For debugging purposes

    private TapGesture tapGesture; // TouchScript's Tap Gesture

    private void OnEnable()
    {
        // Get the TapGesture component and subscribe to the event
        tapGesture = GetComponent<TapGesture>();
        if (tapGesture != null)
        {
            tapGesture.Tapped += OnTap;
            tapGesture.StateChanged += OnGestureStateChanged; // Listen to state changes
        }
    }

    // Handle the gesture state change(for more advanced gesture tracking)
    private void OnGestureStateChanged(object sender, GestureStateChangeEventArgs e)
    {
        // For debugging, check the gesture's state
        //Debug.Log($"Gesture state changed from {e.PreviousState} to {e.State} on {screenName}");
    }

    private void OnDisable()
    {
        // Unsubscribe from eventsks
        if (tapGesture != null)
        {
            tapGesture.Tapped -= OnTap;
            tapGesture.StateChanged -= OnGestureStateChanged;
        }
    }

    private void OnTap(object sender, System.EventArgs e)
    {
        StartGame();
    }

    public void StartGame()
    {
        startUI.SetActive(false);  // Hide start UI
        gameUI.SetActive(true);    // Show in-game UI
        gameObjects.SetActive(true); // Activate game objects
        shopManager.SetActive(true); // Activate shop manager
    }

    public void ReturnToTitlePage()
    {
        gameUI.SetActive(false);
        gameObjects.SetActive(false);
        shopManager.SetActive(false);
        startUI.SetActive(true);
    }
}
