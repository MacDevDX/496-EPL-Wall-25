using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EggDecayer : MonoBehaviour
{
    public List<Edible> edibleList;
    // minimum decay of 1 second.
    public float decayTime;
    public ShopManager shopManagerScript;
    private bool menuIsOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        edibleList = new List<Edible> ();

        shopManagerScript.MenuOpen += HandleMenuOpen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateList()
    {
        if (menuIsOpen)
        {   // If game menu is open, we want to pause game logic
            return; 
        }

        edibleList.RemoveAll(x => !x);
        edibleList.ForEach(delegate (Edible egg)
        {
            if (egg != null) 
            {
                egg.eggDecay += 1;
            }
        });
    }

    void OnEnable()
    {
        float tickTime = decayTime / 5;
        InvokeRepeating("UpdateList", 0, tickTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }
}
