using UnityEngine;
using TouchScript.Gestures.TransformGestures;
using Unity.VisualScripting;

public class AnimatedEgg : MonoBehaviour
{
    private Animator eggAnimator;
    private Rigidbody rb;
    private TransformGesture dragGesture;
    private ShopManager shopManager;

    [Header("Tap Indicator")]
    public GameObject tapTextPrefab;
    public float timeTillIndicator = 30f;
    private bool isTextVisible = false;
    private GameObject tapTextInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopManager = Object.FindFirstObjectByType<ShopManager>();

        float randomY = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(0, randomY, 0);
        eggAnimator = GetComponent<Animator>();
        eggAnimator.SetTrigger("idle");

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;  // Prevent unwanted movement
        }

        if (tapTextPrefab != null)
        {
            tapTextInstance = Instantiate(tapTextPrefab, transform.position + Vector3.up * .8f, Quaternion.Euler(45, 0, 0));
            tapTextInstance.transform.SetParent(transform);
            tapTextInstance.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (tap>=1)
        {
            eggAnimator.SetTrigger("tap");
        }
        */
        if (!isTextVisible)
        {
            timeTillIndicator -= Time.deltaTime;

            if (timeTillIndicator <= 0f)
            {
                ShowTapText();
                timeTillIndicator = 0f;
            }
        }
    }
    public void RegisterTap()
    {
        if (GetComponent<ClicktoHatch>().hatchCountdown >= 1)
        {
            eggAnimator.SetTrigger("tap");
            eggAnimator.SetTrigger("idle");
        }
                
        timeTillIndicator = 30f;  //Reset timer
        HideTapText();
    }
    private void ShowTapText()
    {
        if (tapTextInstance != null)
        {
            tapTextInstance.SetActive(true);
            isTextVisible = true;
        }
    }

    private void HideTapText()
    {
        if (tapTextInstance != null)
        {
            tapTextInstance.SetActive(false);
            isTextVisible = false;
        }
    }

}
