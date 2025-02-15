using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private TransformGesture gesture;

    private void Awake()
    {
        // Ensure TransformGesture exists
        gesture = GetComponent<TransformGesture>() ?? gameObject.AddComponent<TransformGesture>();

        gesture.Transformed += OnDrag;
    }

    private void OnDrag(object sender, System.EventArgs e)
    {
        transform.position += gesture.DeltaPosition;
    }
}
