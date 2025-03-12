using TouchScript.Gestures.TransformGestures;
using TouchScript.Gestures;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BarnDoor : MonoBehaviour
{
    private Animator animator;
    private bool open = false;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        GetComponent<LongPressGesture>().LongPressed += longPressHandler;
    }

    private void OnDisable()
    {
        GetComponent<LongPressGesture>().LongPressed -= longPressHandler;
    }
    private void longPressHandler(object sender, System.EventArgs e)
    {
        if (open)
        {
            animator.SetTrigger("close");
            open = false;
        }
        else
        {
            animator.SetTrigger("open");
            open = true;
        }
    }
}
