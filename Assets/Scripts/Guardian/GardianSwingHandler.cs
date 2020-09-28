using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    //TODO: Be able to put multiple objects with swingtree
    public float swingSpeed;
    // private GameObject[] swingTree; //TODO: will grab from enviroment. Enviroment to update array when it changes, or might grab array from enviroment
    public GameObject swingTree;
    private Vector3 swingPoint;
    private float swingAngle;
    private float swingHeight;


    /* UpdateTreeSwingPoints
      => Loop through available swinging trees and determine if close enough
    */
    private void UpdateSwingPoints(){

      //TODO: Determine how many swingtrees are in play
      // for (int i=0; i<numSwingTrees; i++){
        swingTree.GetComponent<SwingTreeHandler>().CheckDistance(transform.position, clockWiseDirection);

      // }

    }


    /* UseSwingItem

    */
    public void UseSwingItem(){
      //TODO: loop through all the swing trees
      // for (int i=0; i<swingTrees.Length; i++){

        if (swingTree.GetComponent<SwingTreeHandler>().GetSwingable()){

          animator.SetTrigger("swing");
          swingPoint = swingTree.GetComponent<SwingTreeHandler>().GetSwingPoint();

          //Determine angle difference between player and tree
          swingAngle = Vector2.Angle(new Vector2(transform.position.x, transform.position.z), new Vector2(swingPoint.x, swingPoint.z));
          swingHeight = swingPoint.y - transform.position.y;

        }
      // }

      //IDEA: could put a failed item use
      //TODO: will need to add to animator
      // if (curState != state.swing){
      //   Debug.Log("Swing Failed");
      // }

    }


    //TODO: should not be able to collide with enimies when swinging but can still collide with ground obj which should cancel swing
    /* UpdateTreeSwing
      => Check to see if alouted time has passed
      => Update position
    */
    private void UpdateSwing(){

      //TODO: make swing stop if player is past point(x,z) as well

      //Determine player has reached point height
      if (swingPoint.y - transform.position.y <= 0){ //NOTE will need to solve if player is above point



      } else {

        //Determine time to swing
        float swingTime = swingAngle / swingSpeed;

        if (swingTime < 1){
          swingTime = 1;
        }

        //Determine nessisary height intervals
        float swingHeightInterval = swingHeight / swingTime;

        //Update Movement
        rb.velocity = transform.up * swingHeightInterval;
        momentumSpeed = swingSpeed;
        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), swingSpeed * Time.deltaTime);
      }
    }

}
