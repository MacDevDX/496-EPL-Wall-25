using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
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
        //GetComponent<PinchGesture>().StateChanged += pinchHandler;
        //GetComponent<Pinch2>().StateChanged += pinchHandler;
        GetComponent<TransformGesture>().Transformed += pinchHandler;

    }

    private void OnDisable()
    {
        //GetComponent<PinchGesture>().StateChanged -= pinchHandler;
        //GetComponent<Pinch2>().StateChanged -= pinchHandler;
        GetComponent<TransformGesture>().Transformed -= pinchHandler;
    }
    /*
    private void pinchHandler(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized) //When 2 touches pinch..
        {
            Destroy(gameObject);
        }
    }
    */
    private void pinchHandler(object sender, System.EventArgs e)
    {
        var gesture = sender as TransformGesture;
        if (gesture != null)
        {
            float scaleFactor = gesture.DeltaScale;

            if (scaleFactor < 0.8f)
            {
                Destroy(gameObject);
            }

        }
    }
}
