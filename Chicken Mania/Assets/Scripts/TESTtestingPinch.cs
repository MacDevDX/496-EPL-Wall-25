using TouchScript.Gestures;
using UnityEngine;

public class TESTtestingPinch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GetComponent<PinchGesture>().StateChanged += pinchHandler;
        GetComponent<Pinch2>().StateChanged += pinchHandler;
    }

    private void OnDisable()
    {
        GetComponent<PinchGesture>().StateChanged -= pinchHandler;
        GetComponent<Pinch2>().StateChanged -= pinchHandler;
    }

    private void pinchHandler(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized) //When 2 touches pinch..
        {
            Destroy(gameObject);
        }
    }
}
