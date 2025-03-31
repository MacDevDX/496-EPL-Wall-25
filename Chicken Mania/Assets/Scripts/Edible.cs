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
    private bool stale = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Decay", 1.0f, 0.1f);
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

        if (eggDecay >= 3 && !stale)
        {
            if (mat != null && mat2 != null)
            {
                var decayCol1 = new Color(0.098f, 0.196f, 0.11f);
                decayCol1 = Color.Lerp(mat.color, decayCol1, 0.5f);

                mat.SetColor("_BaseColor", decayCol1);
                mat2.SetColor("_BaseColor", decayCol1);

                stale = true;
            }
        }

        if (eggDecay >= 5)
        {
            eaten = true;
        }
    }

}
