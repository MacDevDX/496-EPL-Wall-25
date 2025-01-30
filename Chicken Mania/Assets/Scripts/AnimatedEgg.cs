using UnityEngine;

public class AnimatedEgg : MonoBehaviour
{
    private Animator eggAnimator;
    private int tap = 0; // imported from egg hatch script tbd

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomY = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(0, randomY, 0);
        eggAnimator = GetComponent<Animator>();
        eggAnimator.SetTrigger("idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (tap>=1)
        {
            eggAnimator.SetTrigger("tap");
        }
    }
}
