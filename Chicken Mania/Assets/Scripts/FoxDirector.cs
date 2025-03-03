using UnityEngine;
using System.Collections.Generic;
using System.Linq;


// currently this WILL break when more than one instance of the game is loaded side by side. needs a re-code to fix, some kind of chicken director
// to communicate with or an overarching controller.
public class FoxDirector : MonoBehaviour
{
    public int graceTime;       // the amount of time at the start of the game before foxes will spawn
    public int spawnTick;       // the amount of time between fox spawns
    public int maxFoxes;        // the maximum amount of foxes that can exist
    public float foxSpeed;      // the max speed of the foxes
    public float foxesPer5Chickens;     // the number of foxes that will spawn per 5 chickens owned
    public int devourCooldown;
    public int devourSpreeCD;
    public bool initialFox;     // start with one fox
    public GameObject foxObject;
    public GameObject screenSection;

    // These are public so other classes can communicate. Do not modify these in the inspector!
    public List<Edible> chickenList;
    public List<FoxBehavior> foxList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chickenList = new List<Edible>();
        foxList = new List<FoxBehavior>();

        // this will repeat the listed function. to cancel _all_ invokes, use CancelInvoke()
        InvokeRepeating("UpdateChickenList", graceTime, spawnTick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateChickenList()
    {
        //// get all instances of the chicken script
        //chickenList = Object.FindObjectsOfType<Edible>();

        if (initialFox == true) 
        {
            SpawnFox();
            initialFox = false;
        }
        // remove all null values, eg gameobjects that have been destroyed
        chickenList.RemoveAll(x => !x);

        int spawnTarget = chickenList.Count / 5;
        int foxCount = foxList.Count;
        spawnTarget = (int)(foxesPer5Chickens * spawnTarget);


        while(foxCount < spawnTarget && foxCount < maxFoxes)
        {
            SpawnFox();
            foxCount = foxList.Count;
        }

    }

    public void setupNewEdible(GameObject newObject, ShopManager ShopMan, FoxDirector FoxDir, string edibleType)
    {
        Edible newEdible = newObject.GetComponent<Edible>();
        FoxDir.chickenList.Add(newEdible);
        newEdible.FoxDirecter = FoxDir;
        newEdible.ShopMan = ShopMan;


        if (edibleType == "EGG")
        {
            newEdible.isEgg = true;
        }
        if (edibleType == "CHICK")
        {
            newEdible.isChick = true;
        }
        if (edibleType == "CHICKEN")
        {
            newEdible.isChicken = true;
        }
    }

    void SpawnFox()
    {
        GameObject newFox = Instantiate(foxObject, transform.position, transform.rotation);
        FoxBehavior newFoxScript = newFox.GetComponent<FoxBehavior>();
        newFoxScript.directorRef = this;

        // Tag forx with screen section
        newFox.tag = "Fox_"+screenSection.name;

        foxList.Add(newFoxScript);

        newFoxScript.foxSpeed = foxSpeed;
        newFoxScript.devourCooldown = devourCooldown;
        newFoxScript.devourSpreeCD = devourSpreeCD;

        //Debug.Log("Created fox agent :" + newFoxScript + " for Director :" + this);
    }
}
