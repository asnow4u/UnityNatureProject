using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTreeHandler : MonoBehaviour
{

    private bool swingable;
    private Vector3 swingPoint;
    public float swingRange;

    /* Start

    */
    void Start(){
        swingable = false;

        //TODO: will need to find a good way to find this point
        swingPoint = transform.position + transform.forward * 10 + transform.right * 3 + transform.up * -4;
     }


    /* Update

    */
    void Update(){

    }


    /* DistanceCheck
      => Check position of player and swing point
      => Alter swingable bool accordingly
    */
    public void CheckDistance(Vector3 playerPos, bool clockWise){

      float dist = Vector3.Distance(playerPos, transform.position);

      if (dist <= swingRange){

        //Determine if player is under tree
        float angle = Vector2.Angle(new Vector2(playerPos.x, playerPos.z), new Vector2(swingPoint.x, swingPoint.z));

        if (angle >= 15f ){

          //Determine if direction is towards the tree
          if (TowardsTreeDirection(playerPos, clockWise)){

            //TODO: need an idicator that signifies that swinging is an option

            Debug.Log("Close Enough To Swing");
            swingable = true;

          } else {

            swingable = false;
          }

        } else {

          swingable = false;
        }

      } else {

        swingable = false;
      }
    }


    /* TowardsTreeDirection
      => Determine if player is facing towards the swing tree
      => Based on the position of the swingPoint, determine the quadrent the tree is in
      => Based on quadrent and player direction determine if in sight
    */
    private bool TowardsTreeDirection(Vector3 playerPos, bool clockWise){

      //Top right quadrent
      if (swingPoint.x >= 0 && swingPoint.z >= 0){

        //Headed clockwise
        if (clockWise){
          if (playerPos.x < swingPoint.x && playerPos.z > swingPoint.z){
            return true;
          }

        //Headed counter clockwise
        } else {
          if (playerPos.x > swingPoint.x && playerPos.z < swingPoint.z){
            return true;
          }
        }

      //Top left quadrent
      } else if (swingPoint.x < 0 && swingPoint.z > 0){

        //Headed clockwise
        if (clockWise){
          if (playerPos.x < swingPoint.x && playerPos.z < swingPoint.z){
            return true;
          }

        //Headed counter clockwise
        } else {
          if (playerPos.x > swingPoint.x && playerPos.z > swingPoint.z){
            return true;
          }
        }

      //Bottom right quadrent
      } else if (swingPoint.x > 0 && swingPoint.z < 0){

        //Headed clockwise
        if (clockWise){
          if (playerPos.x > swingPoint.x && playerPos.z > swingPoint.z){
            return true;
          }

        //Headed counter clockwise
        } else {
          if (playerPos.x < swingPoint.x && playerPos.z < swingPoint.z){
            return true;
          }
        }

      //Bottom left quadrent
      } else {

        //Headed clockwise
        if (clockWise){
          if (playerPos.x < swingPoint.x && playerPos.z < swingPoint.z){
            return true;
          }

        //Headed counter clockwise
        } else {
          if (playerPos.x > swingPoint.x && playerPos.z > swingPoint.z){
            return true;
          }
        }
      }

      return false;
    }


    /* GetSwingable
      => returns bool swingable
    */
    public bool GetSwingable(){
      return swingable;
    }


    /* GetSwingPoint
      => Get Vector3 for which to swing off of
    */
    public Vector3 GetSwingPoint(){
      return swingPoint;
    }


    void OnDrawGizmosSelected(){
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position + transform.forward * 10 + transform.right * 3 + transform.up * -4, 1);
    }

}
