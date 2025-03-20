using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BarnSpinner : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

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
        animator.SetTrigger("spin");
        animator.SetTrigger("idle");
    }
}
