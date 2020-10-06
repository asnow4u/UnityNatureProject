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
    private float cursorHeight;
    private Quaternion cursorStartRotation;

    public float cursorRange;
    public float projectileSpeed;
    private float projectileAngle;

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

      switch(curAction){
        case action.water:
          GameObject spawnedWaterProjectile = Instantiate(waterProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);

          if (clockWiseDirection){
            spawnedWaterProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileSpeed, projectileAngle);
            spawnedWaterProjectile.transform.Rotate(Mathf.Abs(projectileAngle - 360), 90f, 0f);
          } else {
            spawnedWaterProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileSpeed, projectileAngle);
            spawnedWaterProjectile.transform.Rotate(projectileAngle, 90f, 0f);
          }

          break;

        case action.thorn:
          GameObject spawnedThornProjectile = Instantiate(thornProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);
          // spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);

          if (clockWiseDirection){
            // spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            // spawnedThornProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedThornProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedThornProjectile.transform.Rotate(-90f, 0f, 0f);
          break;

        case action.yellow:
          GameObject spawnedProjectile = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);
          // spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);

          if (clockWiseDirection){
            // spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            // spawnedProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedProjectile.transform.Rotate(-90f, 0f, 0f);
          break;

        case action.red:
          GameObject spawnedTempProjectile = Instantiate(tempProjectile, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z) + (transform.forward * 2f), Quaternion.identity);


          if (clockWiseDirection){
            // spawnedTempProjectile.GetComponent<GardianProjectile>().SetMoveForces(projectileXSpeed, projectileYSpeed);
          } else {
            // spawnedTempProjectile.GetComponent<GardianProjectile>().SetMoveForces((-1) * projectileXSpeed, projectileYSpeed);
          }

          spawnedTempProjectile.transform.rotation = bowBone.transform.rotation;
          spawnedTempProjectile.transform.Rotate(-90f, 0f, 0f);
          break;
      }
    }



    /* UpdatteAim

    */
    public void UpdateAim(Vector2 aimStick){

      projectileAngle = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;

      //Update angle 0 - 360
      if (projectileAngle < 0f){
        projectileAngle += 360f;

      //Set cursor position
      } else if (projectileAngle == 0){
        if (!clockWiseDirection){
          projectileAngle += 180f;
        }
      }

      //Check to see which side the cursor is on
      if ((clockWiseDirection && projectileAngle > 90f && projectileAngle < 270f) || (!clockWiseDirection && (projectileAngle <= 90f || projectileAngle >= 270f))){
        FlipDirection();
      }

      //Update Cursor
      if (!clockWiseDirection){
        projectileAngle = Mathf.Abs(projectileAngle - 360);
        aimCursor.transform.rotation = Quaternion.AngleAxis(projectileAngle * (-1), transform.right) * transform.rotation * Quaternion.Euler(0, 180f, 0);
        aimCursor.transform.position = new Vector3(transform.position.x, transform.position.y + cursorHeight, transform.position.z) + aimCursor.transform.forward * cursorRange;

      } else {

        //Update Cursor
        aimCursor.transform.rotation = Quaternion.AngleAxis(projectileAngle * (-1), transform.right) * transform.rotation;
        aimCursor.transform.position = new Vector3(transform.position.x, transform.position.y + cursorHeight, transform.position.z) + aimCursor.transform.forward * cursorRange;
      }

      //TODO: Get the bow to point towards the aimCursor
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

}
