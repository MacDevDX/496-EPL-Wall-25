using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;

public class PinchGestureHandler : MonoBehaviour
{
    private TransformGesture transformGesture;
    private float initialScale;

    void OnEnable()
    {
        transformGesture = GetComponent<TransformGesture>();
        if (transformGesture != null)
        {
            transformGesture.TransformStarted += OnTransformStarted;
            transformGesture.Transformed += OnTransformed;
        }
    }

    void OnDisable()
    {
        if (transformGesture != null)
        {
            transformGesture.TransformStarted -= OnTransformStarted;
            transformGesture.Transformed -= OnTransformed;
        }
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        initialScale = transform.localScale.x;
    }

    private void OnTransformed(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        if (gesture != null)
        {
            float scaleFactor = gesture.DeltaScale;
            transform.localScale = new Vector3(initialScale * scaleFactor, initialScale * scaleFactor, initialScale * scaleFactor);
        }
    }
}
