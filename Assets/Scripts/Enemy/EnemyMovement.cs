using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyController : MonoBehaviour
{

    public GameObject gardian;
    public float moveSpeed;
    public bool centerCrossable;
    private bool movingClockWise;

    // private bool crossingPlayerArea;
    // private Vector3 crossingPoint;
    // private GameObject terrainObj;


    /* UpdateDirection

    */
    private void UpdateMovement(){

      //Update Direction
      float angle = Vector3.Angle(transform.position, gardian.transform.position);
      float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.position, gardian.transform.position)));
      angle = (angle * sign + 360) % 360;

      //clockWiseDirection
      if (angle >= 0f && angle <= 180f){
        if (!movingClockWise){
          movingClockWise = !movingClockWise;
          moveSpeed = Mathf.Abs(moveSpeed);
          transform.Rotate(0f, 180f, 0f);
        }

      //Counter clockWiseDirection
      } else {
        if (movingClockWise){
          movingClockWise = !movingClockWise;
          moveSpeed = Mathf.Abs(moveSpeed)*(-1);
          transform.Rotate(0f, 180f, 0f);
        }
      }

      //RotateAround
      transform.RotateAround(Vector3.zero, new Vector3(0,1,0), moveSpeed * Time.deltaTime);

      //Ajust Enemy Model to the terrain
      CalculateAngle();

    }


    /*  CalculateAngle
      => Find the angle to rotate the enemy model based on terrain
      => Uses the normal of the enemy towards the ground to determine the nessisary adjustments
    */
    private void CalculateAngle(){

      RaycastHit hit;
      float angle;

      if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f,  1 << LayerMask.NameToLayer("Ground"))){
        transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
      }
    }


    /* TestCrossingConstraints
      =>Test the following to determine if crossing the center area is a viable option
        => Is player on current quad or the next quad over?
        => Height of terrain enemy is on
    */
    // bool TestCrossingConstraints(){
    //
    //   //TODO: Determine if player is on, or is, one quad away (based on player position)
    //   //TODO: Determine if height of terrain is to high
    //
    //   //Determine if enemy is titan (dosent cross the center)
    //   if (!ableToCrossCenter){
    //     return false;
    //   }
    //
    //   //Randomize if this should happen
    //   int rand = Random.Range(0, 1000);
    //
    //   if (rand < 1){
    //     return true;
    //   } else {
    //     return false;
    //   }
    // }


    /* DetermineCrossing
      => Determine if an availiable spot is open for crossing the center area
    */
    // Vector3 DetermineCrossing(){
    //
    //   int curQuadrent;
    //   int leftQuadrent;
    //   int rightQuadrent;
    //
    //   float dist;
    //
    //   Vector3[] travelPointsLeft;
    //   Vector3[] travelPointsRight;
    //   Vector3 travelPoint;
    //
    //   //Find Current Quadrent
    //   curQuadrent = terrainObj.GetComponent<TerrainMain>().GetCurrentQuadrent(transform.position);
    //
    //   //Find quedrents 3 quads away in both directions.
    //   leftQuadrent = curQuadrent + 3;
    //   if (leftQuadrent > 7){
    //     leftQuadrent -= 8;
    //   }
    //
    //   rightQuadrent = curQuadrent - 3;
    //   if (rightQuadrent < 0){
    //     rightQuadrent += 8;
    //   }
    //
    //   //Find center points
    //   travelPointsLeft = terrainObj.GetComponent<TerrainMain>().GetCenterPointsFromQuad(leftQuadrent);
    //   travelPointsRight = terrainObj.GetComponent<TerrainMain>().GetCenterPointsFromQuad(rightQuadrent);
    //
    //   //Allow crossing of center area
    //   crossingPlayerArea = true;
    //
    //   //Keep looping till targetPoint is returned
    //   while(true){
    //
    //     //Randomly pick 1 of the 6 availiable points
    //     int rand = Random.Range(0, 5);
    //
    //     // rand from travelPointLeft
    //     if (rand < 3){
    //
    //       //Get length of the line from enemy position to targetpoint
    //       dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(travelPointsLeft[rand].x, 0, travelPointsLeft[rand].z));
    //
    //       //Test if line is under 40.0f where the enemy will cross the player area
    //       if (dist < 40.0f){
    //         Debug.Log("line2: " + dist);
    //         return travelPointsLeft[rand];
    //       }
    //
    //     // rand from travelPointsRight
    //     } else {
    //
    //       //Get length of the line from enemy position to targetpoint
    //       dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(travelPointsRight[rand-3].x, 0, travelPointsRight[rand-3].z));
    //
    //       //Test if line is under 40.0f where the enemy will cross the player area
    //       if (dist < 40.0f){
    //         Debug.Log("line2: " + dist);
    //         return travelPointsRight[rand-3];
    //       }
    //     }
    //   }
    // }


    /* GetTransformPosition
      => return current position of enemy
    */
    // public Vector3 GetEnemyPosition(){
    //   return transform.position;
    // }



    /* Crossing Center Left Over Code
      //Check if enemy has reached crossing point
      if ((Mathf.Round(transform.position.x * 10f) / 10f == Mathf.Round(crossingPoint.x * 10f) / 10f) && (Mathf.Round(transform.position.z * 10f) / 10f == Mathf.Round(crossingPoint.z * 10f) / 10f)){
        crossingPlayerArea = false;

      } else {

        //Cross center area
        transform.position = Vector3.MoveTowards(transform.position, crossingPoint, 5 * Time.deltaTime); //TODO Find good speed

        //Adjust Enemy Model
        //TODO Face direction of movement
        CalculateAngle();
      }
    */
}
