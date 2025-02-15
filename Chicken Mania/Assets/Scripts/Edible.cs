using UnityEngine;

public class Edible : MonoBehaviour
{
    public bool isEgg;
    public bool isChick;
    public bool isChicken;
    public bool eaten = false;
    public ParticleSystem Particles;
    public ShopManager ShopMan;
    public FoxDirector FoxDirecter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
