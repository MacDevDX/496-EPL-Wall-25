using TouchScript.Gestures;
using UnityEngine;

public class LongPressHandllerTutorial : MonoBehaviour
{
    private LongPressGesture longPressGesture;

    public GameObject ProtectTutorial;

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
            ProtectTutorial.SetActive(true);
        }
    }
}
