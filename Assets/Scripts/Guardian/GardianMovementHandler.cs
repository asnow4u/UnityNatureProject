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

    public bool sliding;
    public float slidingAcceleration;
    public float slidingForce;
    public float terrainAngle;
    public float steepAngle;

    public bool dashing;
    public bool dashCoolDown;
    public float dashSpeed;
    public float maxDashSpeed;
    public float dashTime;
    public float dashUpwardForce;

    public bool jumping;
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool clockWiseDirection;


    /* UpdateMovement
      => Through Controller input, move player around the center point
      => Call FlipDirection
        => When movement is oppisite of bool clockWiseDirection
        => When movement is slow on an upward slope
      => Adjust moveSpeed based on terrain and controller input
        => Use a sudo drag force to slow down character when moving oppisite direction of bool clockWiseDirection
        => Use a sudo drag force to slow down character when climbing steeper terrain
        => Slow down character when no movement detected from controller
        => Use a sudo drag force to limit movement while in the air
      => Apply moveSpeed to rotating around and call CorrectSlidingYPos
    */
    public void UpdateMovement(float moveStick){

      //Check for direction change
      if ((moveStick > 0f && !clockWiseDirection) || (moveStick < 0f && clockWiseDirection)){
        FlipDirection();
      }

      //Ground movement
      if (IsGrounded()){
        UpdateGroundMovement(moveStick);

      //Air movement
      } else {
        UpdateAirMovement(moveStick);
      }

      //Dash movement
      if (dashing){
        moveSpeed = dashSpeed;

      } else {
        if ((moveSpeed > maxMoveSpeed && clockWiseDirection) || (moveSpeed < -maxMoveSpeed && !clockWiseDirection)){

          moveSpeed = clockWiseDirection ? maxMoveSpeed : (-1)*maxMoveSpeed;
        }
      }

      if (sliding && !jumping){
        CorrectSlidingYPos();
      }

      //Apply moveSpeed
      transform.RotateAround(Vector3.zero, new Vector3(0,1,0), moveSpeed);
    }


    /* UpdateGroundMovement

    */
    private void UpdateGroundMovement(float moveStick){

      if (!dashing){
        LevelToGround();
      }

      if (sliding){                                                                                                                               //State = Sliding
        moveSpeed += (clockWiseDirection ? slidingAcceleration : -slidingAcceleration) * Mathf.Sin(Mathf.Deg2Rad * -terrainAngle);

      } else {                                                                                                                                    //State = walk/run

        if ((moveSpeed > 0 && !clockWiseDirection) || (moveSpeed < 0 && clockWiseDirection)){                                                     //Check if moving the oposite direction to apply more speed
          dragRate = drag;

        } else if (terrainAngle > steepAngle && !dashing){                                                                                                    //Check for steepAngle
          dragRate = -1f - Mathf.Sin(Mathf.Deg2Rad * terrainAngle);

          if (moveSpeed <= 0.1f && clockWiseDirection || moveSpeed >= -0.1f && !clockWiseDirection){
            FlipDirection();
            sliding = true;
          }

        } else {
          dragRate = 1f;
        }

        if (moveStick == 0f){                                                                                                                     //Slow down to a stop
          moveSpeed *= 0.95f;
        } else {
          moveSpeed += accelerationRate * dragRate * moveStick * Time.deltaTime;
        }

        animator.SetFloat("moveStick", Mathf.Abs(moveStick));                                                                                    //Set animation value
      }
    }


    /* UpdateAirMovement

    */
    private void UpdateAirMovement(float moveStick){

      transform.rotation = Quaternion.FromToRotation (transform.up, Vector3.up) * transform.rotation;

      if ((moveSpeed > 0 && !clockWiseDirection) || (moveSpeed < 0 && clockWiseDirection)){                                                      //Set dragRate based on moveStick
        dragRate = airDrag;
        moveSpeed += accelerationRate * dragRate * moveStick * Time.deltaTime;

      } else {
        dragRate = 1f;
      }
    }


    /* Jump
      => Check that character is on the ground
      => Set jumping bool to true (turns to false when character starts to fall)
      => Turns sliding off if character was sliding
      => Apply jumping force in the upward direction
      //TODO: rotate character to no longer be flush with the terrain rather at 0 degrees
    */
    public void Jump(){

      if (IsGrounded() && !jumping){
        jumping = true;
        // animator.SetTrigger("jump");
        // TODO: jump animation will need to be short for a instantaneous reaction to the players input. mostly involving the upper body as to not interupt movement from the legs.

        if (sliding) {
          sliding = false;
        }

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      }
    }


    /* Dash

    */
    public void Dash(Vector2 aimStick){

      if (!dashCoolDown){ //TODO: change to dashAction (cooldown is finished)

        float angle = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;

        if (angle < 0){
          angle += 360f;

        } else if (angle == 0 && !clockWiseDirection){
          angle = 180f;
        }

        dashCoolDown = true;
        dashing = true;
        sliding = false;

        //Angle from projectileAngle
        angle = Mathf.Deg2Rad * angle;

        dashSpeed = Mathf.Cos(angle) * maxDashSpeed;

        float upwardForce = Mathf.Sin(angle) * maxDashSpeed * dashUpwardForce;

        rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

        Invoke("ResetDashCoolDown", dashTime);
      }

      return;
    }


    /* UpdateDash

    */
    private void ResetDashCoolDown(){
      dashCoolDown = false;

      dashing = false; //TODO: should be removed in UpdateMovement when it slows down enough

    }


    /* UpdateDirection
      => Update clockWiseDirection boolean
      => Rotate character 180 degrees
    */
    public void FlipDirection(){

      if (!dashing && !sliding){
        clockWiseDirection = !clockWiseDirection;
        transform.Rotate(0f, 180f, 0f);
      }
    }


    /* LevelToGround
      => Update the terrain angle using a downward raycast
      => Rotate character according to terrain angle
      => Update sliding boolean based on terrain angle
    */
    private void LevelToGround(){

      RaycastHit hit;

      if (Physics.Raycast(transform.position + transform.up, -Vector3.up, out hit, groundDist + 1f,  groundLayer)){

        terrainAngle = Vector3.SignedAngle(Vector3.up, hit.normal, -transform.right);

        transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;

        //Determine if sliding is done
        if (sliding){
          if (terrainAngle > -5f){
            sliding = false;
            //Set animator
          }

        } else {
          //Determine the angle when sliding happens
          if (terrainAngle <= -10f){
            sliding = true;
            //Set animator
          }
        }
      }
    }


    /* CorrectSlidingYPos
      => Prevent boucing when sliding downhill
      => Get distance to ground and adjust character that distance
    */
    private void CorrectSlidingYPos(){
      RaycastHit hit;

      //Raycast down to ground
      if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundDist + 1f,  groundLayer)){
        transform.position = new Vector3(transform.position.x, transform.position.y - hit.distance, transform.position.z);
      }
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
          //TODO: NOT SURE IF NEED TO CHANGE need to prevent when just moving around. check that is grounded is false?, or that jump has been pressed?
          rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        }
      }
    }


    void OnDrawGizmosSelected(){
      Gizmos.color = Color.green;
      Gizmos.DrawLine(transform.position + transform.up, transform.position + transform.up -transform.up * groundDist);
    }
}
