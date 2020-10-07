using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardianProjectile : MonoBehaviour
{

    public GameObject thornsPrefab;
    public LayerMask enemyLayer;

    private float xVelocity;
    private float yVelocity;

    private float angleTrajectory;
    private float angleRate;

    private bool clockWiseDirection;

    private Rigidbody rb;

    private enum projectile {normal, water, thorn, temp}
    private projectile projectileType;


    void Start()
    {
      GetProjectileType();

    }


    /* Update
      => Adjust movement based on xVelocity
      => Adjust trajectory angle using Kinematic equation (Vf = Vi + a * t)
        => Adjust based on direction
        => Time.deltaTime determins the amount of time since last frame
      => Check if enemy is hit
      => Check if ground is hit
    */
    void Update()
    {

        //Update Movement
        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), xVelocity * Time.deltaTime);

        //Update trajectory angle
        angleTrajectory = angleTrajectory + angleRate * Time.deltaTime;

        if (clockWiseDirection){
          transform.rotation = Quaternion.Euler((-1) * angleTrajectory, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        } else {
          transform.rotation = Quaternion.Euler((-1) * angleTrajectory - 180f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        //Check for enemy and deal appropriate damage
        TestEnemyHit();

        //Test if the ground was hit
        if (Physics.Raycast(transform.position, -Vector3.up, 0.5f)){
          DestroyProjectile(true);
        }
    }


    /* GetProjectileType

    */
    private void GetProjectileType(){
      if (gameObject.name == "Projectile(Clone)"){
        projectileType = projectile.normal;

      } else if (gameObject.name == "WaterProjectile(Clone)"){
        projectileType = projectile.water;

      } else if (gameObject.name == "ThornProjectile(Clone)"){
        projectileType = projectile.thorn;

      } else if (gameObject.name == "BigProjectile(Clone)"){
        projectileType = projectile.temp;

      }
    }


    /* DestroyProjectile

    */
    private void DestroyProjectile(bool onGround){

      switch(projectileType){
        case projectile.water:
          //TODO: leave a puddle of water
          break;

        case projectile.thorn:
          if (thornsPrefab != null){

            if (onGround){
              Instantiate(thornsPrefab, transform.position, Quaternion.identity);

            } else {

              RaycastHit hit;
              if (Physics.Raycast(transform.position, -Vector3.up, out hit)){
                Instantiate(thornsPrefab, new Vector3(transform.position.x, transform.position.y - hit.distance, transform.position.z), Quaternion.identity);
              }
            }

            Debug.Log("ThornProjectile");
          }
          break;

        case projectile.temp:

          break;

        default:
          //TODO: will leave projectile on the ground (dosnt do anything just for looks)
          break;
      }

      Destroy(gameObject);
    }



    /* TestEnemyHit

    */
    private void TestEnemyHit(){
      Collider[] enemiesHit = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), 1f, enemyLayer);
      foreach (Collider enemy in enemiesHit){
        enemy.GetComponent<EnemyController>().HitDamage(20);
        DestroyProjectile(false);
      }
    }


    /* SetMoveSpeed
      => Update forces
        => xVelocity and yVelocity
        => Determine direction of arrow (clockwise or counter clockwise)
      => Kinematics to determine the angle rate of change as the arrow flys
        => Vf = Vi + a * t
        => Uses the equation to fist find time when arrows yVelocity is 0 (t = (Vf - Vi) / a)
        => Second time to determine angleRate (a = (Vf - Vi) / t)
      IDEA: Could make default angles (0, 180) be a bit higher for a better firing angle for the arrows trajectory
    */
    public void SetMoveForces(float projectileMoveSpeed, float aimAngle){

      float angle = aimAngle * Mathf.Deg2Rad;
      angleTrajectory = aimAngle;

      //Set Direction and xVelocity
      if (projectileMoveSpeed > 0f){
        clockWiseDirection = true;
        xVelocity = Mathf.Cos(angle) * projectileMoveSpeed;

      } else {
        clockWiseDirection = false;
        xVelocity = Mathf.Cos(angle) * (-1f) * projectileMoveSpeed;
      }

      //Set yVelocity
      yVelocity = Mathf.Sin(angle) * projectileMoveSpeed;
      rb = GetComponent<Rigidbody>();
      rb.AddForce(Vector3.up * yVelocity, ForceMode.Impulse);

      if (yVelocity > 0f){

        //Kinematics to determine time
        float time = ((-1) * yVelocity) / Physics.gravity.y;

        //Kinematics to determine angleRate
        if (clockWiseDirection){
          angleRate = (0f - angleTrajectory) / time;
        } else {
          angleRate = (180f - angleTrajectory) / time;
        }
      }
    }

}
