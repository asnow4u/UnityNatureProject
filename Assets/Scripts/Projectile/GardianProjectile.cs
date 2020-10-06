using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardianProjectile : MonoBehaviour
{

    public GameObject thornsPrefab;
    public LayerMask enemyLayer;

    private float xVelocity;
    private float yVelocity;

    private Rigidbody rb;

    private enum projectile {normal, water, thorn, temp}
    private projectile projectileType;

    // Start is called before the first frame update
    void Start()
    {
      // rb = GetComponent<Rigidbody>();
      GetProjectileType();

    }


    /* Update

    */
    void Update()
    {

        /*TODO: works ok, needs improvements:
            => Get inital launch angle, will take the place of "rot"
            => arrow lifts in the front for the first bit
            => arrow sometimes over rotates
            => arrow sometimes under rotates
        */

        //Update Movement
        transform.RotateAround(Vector3.zero, new Vector3(0,1,0), xVelocity * Time.deltaTime);

        //Update trajectory angle
        // float angle = Mathf.Atan2(rb.velocity.y, moveXSpeed);
        // angle *= Mathf.Rad2Deg;
        //
        // Quaternion rot = transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
        // transform.rotation = rot * Quaternion.AngleAxis(angle, transform.right);


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
      IDEA: Could make default angles (0, 180) be a bit higher for a better firing angle for the arrows trajectory
    */
    public void SetMoveForces(float projectileMoveSpeed, float aimAngle){

      float angle = aimAngle * Mathf.Deg2Rad;

      if (projectileMoveSpeed < 0){
        xVelocity = Mathf.Cos(angle) * (-1) * projectileMoveSpeed;
      } else {
        xVelocity = Mathf.Cos(angle) * projectileMoveSpeed;
      }

      Debug.Log("XV: " + xVelocity + " angle: " + angle + " speed: " + projectileMoveSpeed);


      yVelocity = Mathf.Sin(angle) * projectileMoveSpeed;
      rb = GetComponent<Rigidbody>();
      rb.AddForce(Vector3.up * yVelocity, ForceMode.Impulse);
    }

}
