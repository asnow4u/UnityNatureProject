using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

    public float attackRadius;
    public GameObject projectile;
    public GameObject waterProjectile;
    public GameObject thornProjectile;
    public GameObject tempProjectile;

    public GameObject bowBone;
    public GameObject aimCursor;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public float cursorRange;
    public float projectileSpeed;
    private float projectileXSpeed;
    private float projectileYSpeed;

    private enum action {water, thorn, yellow, red}
    private action curAction;

    public float shootAngle;


    /* UpdateAttackAnimation

    */
    public void UpdateAttackAnimation(){

      //TODO: Add AirAttack
      animator.SetTrigger("attack");
      Invoke("UpdateAttack", 0.3f);
    }


    /* UpdateAttack
      => Check for constraint
      => Check for enemies and deal damage
    */
    public void UpdateAttack(){

      UpdateProjectileSpeed();

      switch(curAction){
        case action.water:
          GameObject spawnedWaterProjectile = Instantiate(waterProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);

          if (clockWiseDirection){
            spawnedWaterProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            spawnedWaterProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedWaterProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedWaterProjectile.transform.Rotate(-90f, 0f, 0f);

          break;

        case action.thorn:
          GameObject spawnedThornProjectile = Instantiate(thornProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);
          spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);

          if (clockWiseDirection){
            spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedThornProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedThornProjectile.transform.Rotate(-90f, 0f, 0f);
          break;

        case action.yellow:
          GameObject spawnedProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);
          spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);

          if (clockWiseDirection){
            spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedProjectile.transform.Rotate(-90f, 0f, 0f);
          break;

        case action.red:
          GameObject spawnedTempProjectile = Instantiate(tempProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);


          if (clockWiseDirection){
            spawnedTempProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            spawnedTempProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedTempProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedTempProjectile.transform.Rotate(-90f, 0f, 0f);
          break;
      }
    }



    /* UpdatteAim

    */
    public void UpdateAim(Vector2 aimStick){

      float angle = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;

      //Update angle 0 - 360
      if (angle < 0f){
        angle += 360f;
      }

      if ((clockWiseDirection && angle > 90f && angle < 270f) || (!clockWiseDirection && (angle <= 90f || angle >= 270f))){
        FlipDirection();
      }

      //TODO fix angle when direction is flipped

      //Update Cursor
      aimCursor.transform.rotation = Quaternion.AngleAxis(angle * (-1), transform.right) * startRotation;
      aimCursor.transform.position = startPosition + aimCursor.transform.forward * cursorRange;

      Debug.Log(angle);


      //Under swinging height
      // if (Physics.Raycast(transform.position, -Vector3.up, 4.5f, groundLayer)){
      //
      //   //Tilt up (0 -> 180)
      //   if (angle >= 0f && angle <= 180f){
      //
      //     //Get range from 0 -> 90
      //     angle /= 2;
      //
      //     //Update bow
      //     bowBone.transform.rotation = Quaternion.AngleAxis(angle * (-1), transform.right);
      //
      //     //Degree to radians
      //     angle = angle * Mathf.Deg2Rad;
      //
      //     projectileXSpeed = Mathf.Cos(angle) * projectileSpeed;
      //     projectileYSpeed = Mathf.Sin(angle) * (projectileSpeed / 2f); //TODO: Work with projectileSpeed to get it to not go as far
      //
      //   //Tilt down (360 -> 180)
      //   } else {
      //
      //     //Get range from 0 -> -90
      //     angle -= 360;
      //     angle /= 2;
      //
      //     //Update bow
      //     bowBone.transform.rotation = Quaternion.AngleAxis(angle * (-1), transform.right);
      //
      //     //Degree to radians
      //     angle = angle * Mathf.Deg2Rad;
      //
      //     projectileXSpeed = Mathf.Cos(angle) * projectileSpeed;
      //     projectileYSpeed = Mathf.Sin(angle) * (projectileSpeed / 2f);
      //
      //   }
      //
      // //In air aiming
      // } else {
      //   //TODO slow down time
      //
      //   //Tilt forward (0 -> 180)
      //   if (angle >= 0f && angle <= 180f){
      //
      //     //Get range from 0 -> 90
      //     angle /= 2f;
      //     angle -= 90f;
      //
      //     //Update bow
      //     bowBone.transform.rotation = Quaternion.AngleAxis(angle * (-1), transform.right);
      //
      //     //Degree to radians
      //     angle = angle * Mathf.Deg2Rad;
      //
      //     projectileXSpeed = Mathf.Cos(angle) * projectileSpeed;
      //     projectileYSpeed = Mathf.Sin(angle) * (projectileSpeed / 2f);
      //
      //   //Tilt backward (360 -> 180)
      //   } else {
      //
      //     //Get range from 0 -> -90
      //     angle -= 360;
      //     angle /= 2;
      //     angle -= 90f;
      //
      //     //Update bow
      //     bowBone.transform.rotation = Quaternion.AngleAxis(angle * (-1), transform.right);
      //
      //     //Degree to radians
      //     angle = angle * Mathf.Deg2Rad;
      //
      //     projectileXSpeed = Mathf.Cos(angle) * projectileSpeed;
      //     projectileYSpeed = Mathf.Sin(angle) * (projectileSpeed / 2f);
      //
      //   }
      //
      // }
    }


    /* UpdateCurrentProjectile

    */
    public void UpdateCurrentProjectile(bool water, bool thorn, bool red, bool yellow){
      if (water){
        curAction = action.water;

      } else if (thorn){
        curAction = action.thorn;

      } else if (red){
        curAction = action.red;

      } else if (yellow){
        curAction = action.yellow;

      }
    }


    /*UpdateProjectileSpeed

    */
    private void UpdateProjectileSpeed(){

      //TODO: Need to determine the apropriate projectile speeds
      // float angle = bowBone.transform.eulerAngles.z;
      // Debug.Log(angle);
      //
      // angle = 360 - angle;
      // Debug.Log(angle);
      //
      // angle /= 4f;
      // Debug.Log("angle2 :" + angle);

      // angle = angle * Mathf.Deg2Rad;

      projectileXSpeed = Mathf.Cos(shootAngle * Mathf.Deg2Rad) * projectileSpeed;
      projectileYSpeed = Mathf.Sin(shootAngle * Mathf.Deg2Rad) * (projectileSpeed / 2f); //TODO: Work with projectileSpeed to get it to not go as far

    }

}
