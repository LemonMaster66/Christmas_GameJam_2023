using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerController playerController; //**Replace This with the Name of Your Player Script**
    private Rigidbody rb;

    [Header("Movement Physics")]
    public float Speed     = 50;
    public float MaxSpeed  = 80;
    public float Gravity   = 100;

    [Header("Properties")]
    public float AgroStart;    //Start Agro Range
    public float AgroEnd;      //Lose Agro Range
    public float AttackRange;  //Attack Range

    [Header("States")]
    public bool Agro          = false;
    public bool KilledPlayer  = false;

    [Header("Debug Stats")]
    public float      VelocityMagnitudeXZ;      //The Rb Velocity Magnitude but only on the X/Z axis (not Y)
    [HideInInspector] public float movementX;   //Movement Input on the X Axis
    [HideInInspector] public float movementY;   //Movement Input on the Z Axis
    [HideInInspector] public Vector3 movement;  //Movement Input Combined


    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>(); //**Replace This with the Name of Your Player Script**
        rb = GetComponent<Rigidbody>();

        //Component Values
        rb.useGravity = false;
    }


    void FixedUpdate()
    {
        #region PerFrame Calculations

            // Calculates the Rotation of the Enemy, Based on its Velocity
            float targetAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Calculates Velocity Magnitude for the X and Z axis
            VelocityMagnitudeXZ = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            VelocityMagnitudeXZ = (float)Math.Round(VelocityMagnitudeXZ, 3);
        #endregion

        //**********************************
        #region Animations
            //Tell me if / when you want to add animations (Ive done it Before)
        #endregion
        //**********************************

        #region Extra Physics Stuff

            //Extra Gravity
            rb.AddForce(Physics.gravity * Gravity /10);

            //Max Speed Cap
            if (rb.velocity.magnitude > MaxSpeed)
            {
                // Get the velocity direction
                Vector3 newVelocity = rb.velocity;
                newVelocity.y = 0f;
                newVelocity = Vector3.ClampMagnitude(newVelocity, MaxSpeed);
                newVelocity.y = rb.velocity.y;
                rb.velocity = newVelocity;
            }
        #endregion
        //**********************************

        //Ignore the rest if The Player is Dead
        if(KilledPlayer) return;
        
        if (Agro)
        {
            //If the enemy is Outsdie a certain Distance from the Player, Lose Agro
            if(Vector3.Distance(transform.position, playerController.gameObject.transform.position) > AgroEnd) LoseAgro();

            //If the enemy is Inside a certain Distance from the Player...
            else
            {
                //Move towards the Player
                movement = (playerController.transform.position - transform.position).normalized;

                //If the enemy is Inside a certain Distance from the Player... Attack
                if(Vector3.Distance(transform.position, playerController.gameObject.transform.position) < AttackRange) EnemyAttack();
            }
        }
        //If the enemy is Inside a certain Distance from the Player, Gain Agro
        else if(Vector3.Distance(transform.position, playerController.gameObject.transform.position) < AgroStart) StartAgro();

        //Rotate In the Direction of Movement
        if(rb.velocity.magnitude > 0.5) transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.1f);
        rb.AddForce(movement * Speed);
    }
    

    public void EnemyAttack()
    {
        playerController.Death(); //Kills the Player
        
        movement = new Vector3(0,0,0); //Stops the Movement

        //Rotates Santa Man to Face The Player
        Vector3 directionToTarget = transform.position - playerController.transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget*-1, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        KilledPlayer = true;
    }


    public void StartAgro()
    {
        Agro = true;
    }
    public void LoseAgro()
    {
        Agro = false;
        movement = new Vector3(0,0,0); //Stop Moving
    }
}
