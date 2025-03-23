using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using static UnityEngine.ParticleSystem;

public class ClickforGold : MonoBehaviour
{
    public float eggTimer = 10f;
    public ShopManager shopManager;
    public FoxDirector FoxDir;

    private Animator eggAnimator;
    public ParticleSystem Particles;

    void Awake()
    {
        eggAnimator = GetComponent<Animator>();
        eggAnimator.SetTrigger("idle");

        TapGesture tapGesture = gameObject.AddComponent<TapGesture>();
        tapGesture.Tapped += OnTouchTap;
    }

    private void OnTouchTap(object sender, System.EventArgs e)
    {
        // Calculate 0.01% of current money or $1 min
        int moneyEarned = Mathf.Max(1, Mathf.FloorToInt(shopManager.Money * 0.001f)); shopManager.Money += moneyEarned;
        shopManager.UpdateUI();

        eggAnimator.SetTrigger("tap");
        eggAnimator.SetTrigger("idle");

    }


    void Update()
    {
        eggTimer -= Time.deltaTime;

        if (eggTimer <= 0f)
        {
            Destroy(gameObject);
            Instantiate(Particles, transform.position, Quaternion.identity);

            shopManager.LoseEgg();
        }
    }
}
