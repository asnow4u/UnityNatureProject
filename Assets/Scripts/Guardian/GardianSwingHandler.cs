using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    private GameObject[] swingTreePoints; //TODO: will grab from enviroment. Enviroment to update array when it changes, or might grab array from enviroment
    private GameObject curSwingPoint;
    // public float swingSpeed;
    public float swingRange;
    private float swingAngle;



    /* UseSwingItem

    */
    public void UseSwingItem(){

      foreach (GameObject swingPoint in swingTreePoints){

        float dist = Vector3.Distance(transform.position, swingPoint.transform.position);
        // Debug.Log("dist: " + dist);

        if (dist <= swingRange && IsFacingTreeDirection(swingPoint)){

          animator.SetTrigger("swing");
          curSwingPoint = swingPoint;

          float heightDiff = curSwingPoint.transform.position.y - transform.position.y;
          swingAngle = Mathf.Asin(heightDiff / dist);

          moveSpeed = maxMoveSpeed * Mathf.Cos(swingAngle);
          moveSpeed = clockWiseDirection ? moveSpeed : (-1)*moveSpeed;
          
          rb.AddForce(Vector3.up * maxMoveSpeed * Mathf.Sin(swingAngle) * 80f, ForceMode.Impulse); // torgue force

        }
      }

      //IDEA: could put a failed item use
      //TODO: will need to add to animator

    }


    /* IsFacingTreeDirection
      => Determine if player is facing towards the swing tree
      => Based on the position of the swingPoint, determine the quadrent the tree is in
      => Based on quadrent and player direction determine if in sight
    */
    private bool IsFacingTreeDirection(GameObject swingPoint){

      //Top half
      if (swingPoint.transform.position.z >= 0 ){

        //Counter Clockwise
        if (!clockWiseDirection && transform.position.x >= swingPoint.transform.position.x){
          return true;
        }

        //Clockwise
        if (clockWiseDirection && transform.position.x <= swingPoint.transform.position.x){
          return true;
        }

      //Bottom half
      } else {

        //Counter Clockwise
        if (!clockWiseDirection && transform.position.x <= swingPoint.transform.position.x){
          return true;
        }

        //ClockWise
        if (clockWiseDirection && transform.position.x >= swingPoint.transform.position.x){
          return true;
        }
      }

      return false;
    }



    //TODO: should not be able to collide with enimies when swinging but can still collide with ground obj which should cancel swing
    /* UpdateTreeSwing
      => Check to see if alouted time has passed
    */
    private void UpdateSwing(){

      //Determine player has reached/passed point
      if (!IsFacingTreeDirection(curSwingPoint)){
        curSwingPoint = null;

      }
    }

}
