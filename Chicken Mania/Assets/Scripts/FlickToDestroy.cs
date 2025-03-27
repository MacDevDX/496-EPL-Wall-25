using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FlickToDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<FlickGesture>().Flicked += flickHandler;
    }

    private void OnDisable()
    {
        var flickGesture = GetComponent<FlickGesture>();
        GetComponent<FlickGesture>().Flicked -= flickHandler;
    }
    private void flickHandler(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // Deactivate all child
        }
    }
}

