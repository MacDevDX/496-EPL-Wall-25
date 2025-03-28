using UnityEngine;
using TouchScript.Gestures;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.ParticleSystem;
using TouchScript.Gestures.TransformGestures;


public class FoxBehavior : MonoBehaviour
{
    public int tapsRequired;
    public float foxSpeed;
    public float devourCooldown;
    public float devourSpreeCD;
    public ParticleSystem Particles;

    public FoxDirector directorRef;
    
    public Edible chickenTarget;
    private Rigidbody chickenBody;
    private Rigidbody rBody;
    private Animator animator;

    private Vector3 wanderTarget;
    private float wanderCooldown = 0;

    private bool menuIsOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("chasing", false);

        directorRef.shopManagerScript.MenuOpen += HandleMenuOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuIsOpen)
        {
            return;
        }

        if (chickenTarget != null)
        {
            // aquire the vector pointing to the target
            Vector3 targetVector = chickenBody.position - rBody.position;

            if (rBody.linearVelocity.magnitude < foxSpeed)
            {
                Vector3 targetVecNorm = Vector3.Normalize(targetVector);

                // move towards the target
                rBody.AddForce(targetVecNorm * 1200 * Time.deltaTime);

                // rotate towards the target
                targetVecNorm.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(targetVecNorm, Vector3.up);
                // this second rotation offsets the rotation so the object points "forward"
                targetRotation = targetRotation * Quaternion.Euler(0,0,0);
                rBody.MoveRotation(targetRotation);

            }

            // cooldown is measured in frames, very bad! switch to time later
            if (devourCooldown <= 0)
            {
                devourCooldown = devourSpreeCD;
                chickenTarget.eaten = true;

            }

            if (targetVector.magnitude < 1.5)
            {
                devourCooldown -= Time.deltaTime;
            }
        }
        else
        {
            if (directorRef.chickenList.Count() > 0)
            {
                AquireTarget();
            }
            else WanderRandomly();
        }





    }

    // Pick a random target script instance from the FoxDirector and assign its reference and its rigidbody's reference to this class
    void AquireTarget()
    {
        int count = directorRef.chickenList.Count();
        animator.SetBool("chasing", false);

        if (count > 0)
        {
            int choice = Random.Range(0, count);
            chickenTarget = directorRef.chickenList[choice];

            if (chickenTarget != null)
            {
                chickenBody = chickenTarget.gameObject.GetComponent<Rigidbody>();
                animator.SetBool("chasing", true);

                //if (chickenTarget.isChicken)
                //{
                //    // formally declare war upon the chicken
                //    chickenTarget.gameObject.GetComponent<AnimatedChickenAI1>().chasingFox = this;
                //}
            }
        }

    }

    void WanderRandomly()
    {
        if (wanderCooldown <= 0)
        {
            // 50% idle or wander
            if (Random.Range(0, 2) == 0)
            {
                // idle duration and set animation (currently only one state) false
                wanderCooldown = Random.Range(2f, 4f);
                animator.SetBool("chasing", false); 
            }
            else
            {
                // wander set distance from spawn
                Vector3 randomDirection = new Vector3(
                    Random.Range(-5f, 5f),
                    0,
                    Random.Range(-5f, 5f)
                );
                wanderTarget = rBody.position + randomDirection;

                // time to set next coordinate for wander
                wanderCooldown = Random.Range(2f, 4f);
            }
        }
        else
        {
            wanderCooldown -= Time.deltaTime;

            // moving to coordinate
            if (wanderTarget != Vector3.zero)
            {
                // sets the chasing animation
                Vector3 wanderVector = wanderTarget - rBody.position;
                animator.SetBool("chasing", true);
                if (wanderVector.magnitude > 0.5f)
                {
                    Vector3 wanderNorm = Vector3.Normalize(wanderVector);
                    rBody.AddForce(wanderNorm * foxSpeed * 200 * Time.deltaTime); //(wanderNorm * foxSpeed * Time.deltaTime);

                    // rotation
                    wanderNorm.y = 0;
                    Quaternion wanderRotation = Quaternion.LookRotation(wanderNorm, Vector3.up);
                    rBody.MoveRotation(wanderRotation);
                }
                else
                {
                    wanderTarget = Vector3.zero; // reset once reached coordinate
                }
            }
        }
    }
    /*
    void WanderRandomly()
    {
        if (wanderCooldown <= 0)
        {
            // Pick a random direction within a range
            Vector3 randomDirection = new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );
            wanderTarget = rBody.position + randomDirection;

            // Reset cooldown for the next wander
            wanderCooldown = Random.Range(2f, 4f);
        }
        else
        {
            wanderCooldown -= Time.deltaTime;

            // Move towards the wander target
            Vector3 wanderVector = wanderTarget - rBody.position;

            if (wanderVector.magnitude > 0.5f)
            {
                Vector3 wanderNorm = Vector3.Normalize(wanderVector);
                rBody.AddForce(wanderNorm * 600 * Time.deltaTime);

                // Rotate towards the wander target
                wanderNorm.y = 0;
                Quaternion wanderRotation = Quaternion.LookRotation(wanderNorm, Vector3.up);
                rBody.MoveRotation(wanderRotation);
            }
        }
    }
    */
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fence"))
        {
            wanderCooldown = 0;
        }
    }



    void HandleMenuOpen(object sender, MenuOpenEventArgs a)
    {
        menuIsOpen = a.State;
    }



    // This block is touchscript handling -----------------------------------

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
        //GetComponent<FlickGesture>().Flicked += flickHandler;
        GetComponent<TransformGesture>().Transformed += pinchHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
        //GetComponent<FlickGesture>().Flicked -= flickHandler;
        GetComponent<TransformGesture>().Transformed -= pinchHandler;
    }


    private void tappedHandler(object sender, System.EventArgs e)
    {
    tapsRequired -= 1;
    if (tapsRequired <= 0)
        {
            directorRef.foxList.Remove(this);
            // notify the chicken that it is safe
            //if (chickenTarget != null && chickenTarget.isChicken)
            //{
            //    chickenTarget.gameObject.GetComponent<AnimatedChickenAI1>().chasingFox = null; 
            //}
            //Debug.Log("Fox(" + this + ") has notified :" + directorRef + " of its termination.");
            Destroy(gameObject);
            Instantiate(Particles, transform.position, Quaternion.identity);
        }
    }
    /*
    private void flickHandler(object sender, System.EventArgs e)
    {
        directorRef.foxList.Remove(this);
        Destroy(gameObject);
        Instantiate(Particles, transform.position, Quaternion.identity);
    }
    */
    private void pinchHandler(object sender, System.EventArgs e)
    {
        directorRef.foxList.Remove(this);
        Destroy(gameObject);
        Instantiate(Particles, transform.position, Quaternion.identity);
    }

    // End of touchscript ---------------------------------------------------
}
