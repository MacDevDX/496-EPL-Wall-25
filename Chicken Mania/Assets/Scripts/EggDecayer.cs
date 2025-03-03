using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EggDecayer : MonoBehaviour
{
    public List<Edible> edibleList;
    public float decayTickTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 
        edibleList = new List<Edible> ();
        InvokeRepeating("UpdateList", 0, decayTickTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateList()
    {
        edibleList.RemoveAll(x => !x);
        edibleList.ForEach(delegate (Edible egg)
        {
            if (egg != null) 
            {
                egg.eggDecay += 1;
            }
        });
    }
}
