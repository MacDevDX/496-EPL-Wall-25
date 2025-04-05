using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TouchScript.Examples.Tap;


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
    public ShopManager shopManagerScript;
    private bool menuIsOpen = false;

    // These are public so other classes can communicate. Do not modify these in the inspector!
    public List<Edible> chickenList;
    public List<FoxBehavior> foxList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chickenList = new List<Edible>();
        foxList = new List<FoxBehavior>();
        shopManagerScript.MenuOpen += HandleMenuOpen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChickenList()
    {
        //// get all instances of the chicken script
        //chickenList = Object.FindObjectsOfType<Edible>();
        if (menuIsOpen)
        {
            return;
        }

        if (initialFox == true) 
        {
            SpawnFox();
            initialFox = false;
        }
        // remove all null values, eg gameobjects that have been destroyed
        chickenList.RemoveAll(x => !x);
        foxList.RemoveAll(x => !x);

        int spawnTarget = chickenList.Count / 5;
        int foxCount = foxList.Count;
        spawnTarget = (int)(foxesPer5Chickens * spawnTarget);

        while (foxCount < spawnTarget && foxCount < maxFoxes)
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

    public void SpawnFox()
    {    
        // Define the spawn range
        float spawnRangeX = 4f; // Range for X-axis
        float spawnRangeZ = 4f; // Range for Z-axis

        // Randomize the spawn position within the range
        Vector3 randomizedPosition = new Vector3(
            transform.position.x + Random.Range(-spawnRangeX, spawnRangeX),
            transform.position.y, // Maintain the original Y position
            transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ)
        );

        GameObject newFox = Instantiate(foxObject, randomizedPosition, transform.rotation);
        FoxBehavior newFoxScript = newFox.GetComponent<FoxBehavior>();
        newFoxScript.directorRef = this;
        newFox.transform.SetParent(screenSection.transform);


        // Tag forx with screen section
        newFox.tag = "Fox_"+screenSection.name;

        foxList.Add(newFoxScript);

        newFoxScript.foxSpeed = foxSpeed;
        newFoxScript.devourCooldown = devourCooldown;
        newFoxScript.devourSpreeCD = devourSpreeCD;

        //Debug.Log("Created fox agent :" + newFoxScript + " for Director :" + this);
    }

    void OnEnable()
    {
        // this will repeat the listed function. to cancel _all_ invokes, use CancelInvoke()
        InvokeRepeating("UpdateChickenList", graceTime, spawnTick);
    }

    void OnDisable()
    {
        CancelInvoke();
        chickenList.Clear();
        foxList.Clear();
    }

    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }

}

