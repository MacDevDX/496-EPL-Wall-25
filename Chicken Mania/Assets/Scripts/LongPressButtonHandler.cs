using UnityEngine;
using TouchScript.Gestures;

public class LongPressButtonHandler : MonoBehaviour
{
    private LongPressGesture longPressGesture;

    public ScreenController screenController;

    private void OnEnable()
    {
        longPressGesture = GetComponent<LongPressGesture>();
        if (longPressGesture != null)
        {
            longPressGesture.StateChanged += OnLongPress;
        }
    }

    private void OnDisable()
    {
        if (longPressGesture != null)
        {
            longPressGesture.StateChanged -= OnLongPress;
        }
    }

    private void OnLongPress(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized)
        {
            screenController.ProtectGameMode(); // Call the function from ScreenController
        }
    }
}
