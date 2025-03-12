using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine;

public class BarnBell : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void pressHandler(object sender, System.EventArgs e)
    {
        animator.SetBool("Ring", true);
    }

    private void releaseHandler(object sender, System.EventArgs e)
    {
        animator.SetBool("Ring", false);
    }
}