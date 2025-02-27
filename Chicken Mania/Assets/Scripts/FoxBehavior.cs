using UnityEngine;
using TouchScript.Gestures;
using System.Collections.Generic;
using System.Linq;


public class FoxBehavior : MonoBehaviour
{
    public int tapsRequired;
    public float foxSpeed;
    public float devourCooldown;
    public float devourSpreeCD;

    public FoxDirector directorRef;
    
    public Edible chickenTarget;
    private Rigidbody chickenBody;
    private Rigidbody rBody;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
                targetRotation = targetRotation * Quaternion.Euler(-90,0,-90);
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
            AquireTarget();
        }

    



    }

    // Pick a random target script instance from the FoxDirector and assign its reference and its rigidbody's reference to this class
    void AquireTarget()
    {
        int count = directorRef.chickenList.Count();

        if (count > 0)
        {
            int choice = Random.Range(0, count);
            chickenTarget = directorRef.chickenList[choice];

            if (chickenTarget != null)
            {
                chickenBody = chickenTarget.gameObject.GetComponent<Rigidbody>();

                //if (chickenTarget.isChicken)
                //{
                //    // formally declare war upon the chicken
                //    chickenTarget.gameObject.GetComponent<AnimatedChickenAI1>().chasingFox = this;
                //}
            }
        }

    }









    // This block is touchscript handling -----------------------------------

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
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
            Debug.Log("Fox(" + this + ") has notified :" + directorRef + " of its termination.");
            Destroy(gameObject);
        }
    }

    // End of touchscript ---------------------------------------------------
}
