using UnityEngine;

public class Edible : MonoBehaviour
{
    public bool eaten = false;
    public ParticleSystem Particles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eaten == true)
        {
            Instantiate(Particles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }
}
