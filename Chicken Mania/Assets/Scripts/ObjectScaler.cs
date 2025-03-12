using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    private TransformGesture scaleGesture;
    private Vector3 initialScale;
    public float maxSize = 2f;
    public float minSize = 1f;

    private void OnEnable()
    {
        scaleGesture = GetComponent<TransformGesture>();
        initialScale = transform.localScale;
        scaleGesture.Transformed += Scaling;

    }
    private void Scaling(object sender, System.EventArgs e)
    {
        float newScaleFactor = Mathf.Clamp(transform.localScale.x * scaleGesture.DeltaScale, minSize, maxSize);
        transform.localScale = initialScale * newScaleFactor;
    }
    private void LateUpdate()
    {
        //enforce scale in case it's overridden visually from TouchScript's DeltaScale
        transform.localScale = new Vector3(
            Mathf.Clamp(transform.localScale.x, 1f, 2f),
            Mathf.Clamp(transform.localScale.y, 1f, 2f),
            Mathf.Clamp(transform.localScale.z, 1f, 2f)
        );
    }
}
