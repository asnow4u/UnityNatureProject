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

    private Vector3 preLocation;
    private float distanceTraveled;


    /* Start
      => Initialize Variables
    */
    void Start()
    {
      // health = 100;
      // speed = 10.0f;
      // jumpForce = 8.0f;
      // fallForce = 2.0f;
      distanceTraveled = 0.0f;
      preLocation = transform.position;
      clockWiseDirection = true;

      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody>();
    }


    /* Update
      => Test GameOver Constraints (Health < 0)
      => Grab input from user
      => Update forces
      => Calculate total movement of player
    */
    void Update()
    {

      // Test if players health is at or below 0
      if (health <= 0){
        Debug.Log("Dead!");
      }

      //Check if swinging
      if (animator.GetCurrentAnimatorStateInfo(0).IsName("Swing")){
        UpdateSwing();
      }


      UpdateForces();
      UpdateProgression();
      UpdateSwingPoints();

      //Update isGrounded bool with animator
      if (IsGrounded()){
        animator.SetBool("isGrounded", true);
      } else {
        animator.SetBool("isGrounded", false);
      }

      //TEST
      Debug.Log("Grounded: " + IsGrounded());

    }


    //TODO: Either here or in a new function, apply small forces to nudge the player back to the center if they are off.
    /* UpdateForces
      => Update any forces on player
      => Implement gravity force if player is falling
    */
    private void UpdateForces(){
      // Downward force in the air
      if (rb.velocity.y < 0 && !IsGrounded()){
        rb.velocity += Vector3.up * Physics.gravity.y * fallForce * Time.deltaTime;

      //Applying continual jump force
      } else if (rb.velocity.y > 0 && OVRInput.Get(OVRInput.RawButton.LIndexTrigger)){
        rb.velocity += Vector3.up * Physics.gravity.y * fallForce / 2 * Time.deltaTime;
      }
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
      return Physics.Raycast(transform.position, -Vector3.up, 1, groundLayer);
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


    // void OnDrawGizmosSelected(){
      // Gizmos.color = Color.red;
      // Gizmos.DrawWireSphere(transform.position + transform.forward * 2.5f + transform.up * 2, attackRadius);
      // Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * 5f + transform.up * 9f + transform.right,
      //                               Quaternion.identity,
      //                               new Vector3(7, 4, 1));
      // Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    // }

}
