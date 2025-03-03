using UnityEngine;

public class Edible : MonoBehaviour
{
    public bool isEgg;
    public bool isChick;
    public bool isChicken;
    public bool eaten = false;
    public int eggDecay = 0;
    public ParticleSystem Particles;
    public ShopManager ShopMan;
    public FoxDirector FoxDirecter;

    private Material mat;
    private Material mat2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Decay", 1.0f, 0.5f);
        // store the material of this object for decay
        var renderers = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (renderers.Length >= 2 )
        {
            mat = renderers[0].material;
            mat2 = renderers[1].material;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (eaten == true)
        {
            //calls shopmanager functions to decrement counts of the relevant eaten item
            if (isChicken == true)
            {
                ShopMan.SellChicken();
            }
            if (isChick == true)
            {
                ShopMan.SellChick();
            }
            if (isEgg == true)
            {
                ShopMan.SellEgg();
            }

            Instantiate(Particles, transform.position, Quaternion.identity);
            FoxDirecter.chickenList.Remove(this);
            Destroy(this.gameObject);

        }

    }

    void Decay()
    {
        if (!isEgg)
        {
            CancelInvoke();
        }

        if (eggDecay == 2)
        {
            var decayCol1 = Color.white;
            decayCol1.a = 0.1f;

            mat.SetColor("_BaseColor", decayCol1);
            mat2.SetColor("_BaseColor", decayCol1);
        }

        if (eggDecay == 3)
        {
            var decayCol2 = Color.white;
            decayCol2.a = 0.03f;

            mat.SetColor("_BaseColor", decayCol2);
            mat2.SetColor("_BaseColor", decayCol2);
        }

        if (eggDecay >= 4)
        {
            eaten = true;
        }
    }
}
