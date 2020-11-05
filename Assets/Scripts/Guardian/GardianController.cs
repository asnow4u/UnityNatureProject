using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public partial class GardianController : MonoBehaviour
{

    public int health;
    private Animator animator;
    private Rigidbody rb;
    public LayerMask groundLayer;

    public float groundDist;

    private Vector3 preLocation;
    private float distanceTraveled;


    /* Start
      => Initialize Variables
    */
    void Start()
    {

      distanceTraveled = 0.0f;
      preLocation = transform.position;
      clockWiseDirection = true;
      sliding = false;

      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody>();

      cursorHeight = aimCursor.transform.position.y - transform.position.y;
      cursorStartRotation = aimCursor.transform.rotation;

      //TODO: the swingPoints will come from
      swingTreePoints = GameObject.FindGameObjectsWithTag("SwingPoint");
    }


    /*
      => Update forces
    */
    void FixedUpdate(){

      UpdateForces();
    }


    /* Update
      => Test GameOver Constraints (Health < 0)
      => Grab input from user

      => Calculate total movement of player
    */
    void Update()
    {

      // Test if players health is at or below 0
      if (health <= 0){
        Debug.Log("Dead!");
      }


      //UpdateProgression();

      //Update isGrounded bool with animator
      // if (IsGrounded()){
      //   animator.SetBool("isGrounded", true);
      // } else {
      //   animator.SetBool("isGrounded", false);
      // }

      //Determine if character is falling
      if (jumping && (Mathf.Round(rb.velocity.y * 100f) / 100f) < 0){
        jumping = false;
      }

      //TEST
      Debug.Log("Grounded: " + IsGrounded());

    }


    //TODO: Gonna need to find away to prevent the player from moving back and forth to build distance
    /* UpdateProgression
      => Find distance traveled from previous frame
    */
    private void UpdateProgression(){

      // Update players distance traveled based on angle from a previous location
      preLocation = transform.position;
      float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(preLocation.x, 0, preLocation.z));
      float centerDist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(0, 0, 0));
      float angle = Mathf.Asin(dist / centerDist);

      if (angle < 0){
        angle = angle*(-1);
      }

      distanceTraveled += angle;
    }


    /* TestOnGround
      => Check that the player is on the ground
    */
    public bool IsGrounded(){
      return Physics.Raycast(transform.position + transform.up, -transform.up, groundDist, groundLayer);
    }


    /* GetPlayerHealth
      => Return the currents players health
    */
    public int GetPlayerHealth(){
      return health;
    }


    /* GetProgression
      => Return how far the player has traveled
    */
    public float GetProgression(){
      return distanceTraveled;
    }


    /* OnCollisionEnter

    */
    // void OnCollisionEnter(Collision col){
    //
    //   //When an enemy hits player
    //   if (col.gameObject.tag == "Enemy"){
    //
    //     //TODO: determine type of enemy to determine how much damage is dealt
    //     health -= 10;
    //
    //   }
    // }

}
