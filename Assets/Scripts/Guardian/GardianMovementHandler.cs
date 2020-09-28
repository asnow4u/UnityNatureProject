using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    private float momentumSpeed;
    public float moveSpeed;
    public float jumpForce;
    public float fallForce;
    private bool clockWiseDirection;

    /* UpdateMovement
      => Through Controller input, move player around the center point
      => Move speed based on state
      => Update Animation
    */
    public void UpdateMovement(float moveStick){

      //Ground movement
      if (IsGrounded()){

        UpdateDirection(moveStick);

        //Set animation value
        if (clockWiseDirection){
          animator.SetFloat("moveStick", moveStick);
        } else {
          animator.SetFloat("moveStick", moveStick * (-1));
        }

        //Attack state
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")){
          momentumSpeed = moveStick * moveSpeed/2;

        //Dodge state
        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge")){
          momentumSpeed = moveStick * moveSpeed/2;

        } else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Item") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Swing")){
          momentumSpeed = moveStick * moveSpeed;
        }

        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), momentumSpeed * Time.deltaTime);


      //Air movement
      } else {

        UpdateDirection(moveStick);

        //TODO: make slight adjustments based on user input (Test)
        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), (momentumSpeed) * Time.deltaTime);
      }
    }


    /* UpdateJump

    */
    public void UpdateJump(){

      if (IsGrounded()){
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Movement")){
          animator.SetTrigger("jump");

          //TODO: jump animation will need to be short for a instantaneous reaction to the players input. mostly involving the upper body as to not interupt movement from the legs.

          rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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
    public void UpdateDirection(float moveStick){

      //ClockWise direction
      if (clockWiseDirection){

        if (moveStick < 0){
          clockWiseDirection = false;
          transform.Rotate(0f, 180f, 0f);
        }

      //Counter clockwise direction
      } else {

        if (moveStick > 0){
          clockWiseDirection = true;
          transform.Rotate(0f, 180f, 0f);
        }

      }

    }
}
