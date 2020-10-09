using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    private GameObject[] swingTreePoints;
    private GameObject curSwingPoint;
    // public float swingSpeed;
    public float swingRange;
    private float swingAngle;


    //IDEA: When above swing point dont grapple, instead swing
    //TODO: points are static and will adjust based on terrain height
    /* UseSwingItem

    */
    public void UseSwingItem(){

      swingTreePoints = GameObject.FindGameObjectsWithTag("SwingPoint"); //TODO: grab from enviroment instead?

      foreach (GameObject swingPoint in swingTreePoints){

        float dist = Vector3.Distance(transform.position, swingPoint.transform.position);

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

    */
    private bool IsFacingTreeDirection(GameObject swingPoint){

      float xPos = swingPoint.transform.position.x;

      switch (swingPoint.transform.position.z){
        //Top
        case 80:  if (clockWiseDirection){
                    if (xPos >= transform.position.x){
                      return true;
                    }

                  } else {
                    if (xPos < transform.position.x){
                      return true;
                    }
                  }
                  break;

        //Top half
        case 40:  if (clockWiseDirection){

                    //Right
                    if (xPos > 0 && xPos >= transform.position.x){
                      return true;

                    //Left
                  } else if (xPos < 0 && xPos >= transform.position.x){
                      return true;
                    }

                  //Counter clockwise
                  } else {

                    //Right
                    if (xPos > 0 && xPos < transform.position.x){
                      return true;

                    //Left
                  } else if (xPos < 0 && xPos < transform.position.x){
                      return true;
                    }
                  }
                  break;

        //Bottom half
        case -40: if (clockWiseDirection){

                    //Right
                    if (xPos > 0 && xPos <= transform.position.x){
                      return true;

                    //Left
                  } else if (xPos < 0 && xPos <= transform.position.x){
                      return true;
                    }

                  //Counter clockwise
                  } else {

                    //Right
                    if (xPos > 0 && xPos > transform.position.x){
                      return true;

                    //Left
                  } else if (xPos < 0 && xPos > transform.position.x){
                      return true;
                    }
                  }
                  break;

        //Bottom
        case -80: if (clockWiseDirection){
                    if (xPos <= transform.position.x){
                      return true;
                    }

                  } else {
                    if (xPos > transform.position.x){
                      return true;
                    }
                  }
                  break;
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
