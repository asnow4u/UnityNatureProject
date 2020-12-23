using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGruntFollowerController : MonoBehaviour
{

    public int health;
    public float moveSpeed;
    public float leaderDistance;
    public int directionScalar;
    public GameObject gruntLeader;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


      UpdateMovement();

    }


    /*UpdateMovement

    */
    private void UpdateMovement(){

      if (gruntLeader != null){

        DetermineDirection();

        if (Vector3.Distance(gruntLeader.transform.position, transform.position) > leaderDistance){
          moveSpeed = 5f;

        } else {
          //Debug.Log("Distance:" + Vector3.Distance(gruntLeader.transform.position, transform.position));
          moveSpeed = 0f;
        }

        //rotate around
        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), moveSpeed * directionScalar * Time.deltaTime);

        GroundAngle();
      }
    }


    /* DetermineDirection

    */
    private void DetermineDirection(){

      float angle = Vector3.Angle(transform.position, gruntLeader.transform.position);
      float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.position, gruntLeader.transform.position)));
      angle = (angle * sign + 360) % 360;

      //clockWiseDirection
      if (angle >= 0f && angle <= 180f){
        if (directionScalar < 0){
          directionScalar = 1;
          transform.Rotate(0f, 180f, 0f);

        } else if (directionScalar == 0){
          directionScalar = 1;
          transform.Rotate(0f, -90f, 0f);
        }

      //Counter clockWiseDirection
      } else {
        if (directionScalar > 0){
          directionScalar = -1;
          transform.Rotate(0f, 180f, 0f);

        } else if (directionScalar == 0){
          directionScalar = -1;
          transform.Rotate(0f, 90f, 0f);
        }
      }
    }


    /* GroundAngle
      => Find the angle to rotate the enemy model based on terrain
      => Uses the normal of the enemy towards the ground to determine the nessisary adjustments
    */
    private void GroundAngle(){

      RaycastHit hit;
      float angle;

      if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f,  1 << LayerMask.NameToLayer("Ground"))){
        transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
      }
    }


    /* DesignateLeader

    */
    public void DesignateLeader(GameObject grunt){
      gruntLeader = grunt;
    }
}
