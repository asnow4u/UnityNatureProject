using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    private float momentumSpeed;

    public float maxMoveSpeed;
    public float accelerationRate;
    private float dragRate;
    public float drag;
    public float airDrag;
    public float moveSpeed;

    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    private bool clockWiseDirection;


    /* UpdateMovement
      => Through Controller input, move player around the center point
      => Move speed based on state
      => Update Animation
    */
    public void UpdateMovement(float moveStick){

      //Check for direction change
      if ((moveStick > 0f && !clockWiseDirection) || (moveStick < 0f && clockWiseDirection)){
        FlipDirection();
      }

      //Check if swinging
      if (curSwingPoint == null){

        //Ground movement
        if (IsGrounded()){

          //Set dragRate based on moveStick
          if ((moveSpeed > 0 && !clockWiseDirection) || (moveSpeed < 0 && clockWiseDirection)){
            dragRate = drag;
            //TODO: Set animator for slide stop animation

          } else {
            dragRate = 1f;
          }

          //Slow down to a stop
          if (moveStick == 0f){
            moveSpeed *= 0.95f;

          } else {
            moveSpeed += accelerationRate * dragRate * moveStick * Time.deltaTime;
          }

          //Set animation value
          animator.SetFloat("moveStick", Mathf.Abs(moveStick));

        //Air movement
        } else {

          //Set dragRate based on moveStick
          if ((moveSpeed > 0 && !clockWiseDirection) || (moveSpeed < 0 && clockWiseDirection)){
            dragRate = airDrag;
            moveSpeed += accelerationRate * dragRate * moveStick * Time.deltaTime;

          } else {
            dragRate = 1f;
          }
        }
      }


      //Check if above maxspeed
      if (Mathf.Abs(moveSpeed) > maxMoveSpeed){
        moveSpeed = clockWiseDirection ? maxMoveSpeed : (-1)*maxMoveSpeed;
      }

      transform.RotateAround(Vector3.zero, new Vector3(0,1,0), moveSpeed);
    }


    /* UpdateJump

    */
    public void Jump(){

      if (IsGrounded()){
        animator.SetTrigger("jump");
        //TODO: jump animation will need to be short for a instantaneous reaction to the players input. mostly involving the upper body as to not interupt movement from the legs.

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      }
    }


    /* UpdateDodge

    */
    public void UpdateDodge(){
      if (IsGrounded()){
        animator.SetTrigger("dodge");
      }
    }


    /* UpdateDirection

    */
    public void FlipDirection(){

        clockWiseDirection = !clockWiseDirection;
        transform.Rotate(0f, 180f, 0f);
    }


    /* UpdateForces
      => Update any forces on player
      => Implement gravity force if player is falling
    */
    //TODO: Either here or in a new function, apply small forces to nudge the player back to the center if they are off.
    private void UpdateForces(){

      //Swing Force
      if (curSwingPoint != null){
        UpdateSwing();

      //Small Jump / Fall force
      } else {
        if (rb.velocity.y < 0){
          rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        } else if (rb.velocity.y > 0 && !OVRInput.Get(OVRInput.RawButton.A)){
          rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
      }
    }


}
