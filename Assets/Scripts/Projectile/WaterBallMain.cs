using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBallMain : MonoBehaviour
{
    private float heightDiff;


    // Start is called before the first frame update
    void Start()
    {
        //TODO: be able to get the nessisary information from playerController

        //use Kinematics to determine where ball will land
        heightDiff = GetHeightDifference();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private float GetHeightDifference(){

      float angle;
      float x;
      float z;

      //TODO: get the height diff from where the ball starts to the terrain height
      x = (transform.position.x ) - transform.position.x;
      z = (transform.position.z ) - transform.position.z;


      angle = Mathf.Atan(z / x);

      x = 20 * Mathf.Cos(angle);
      z = 20 * Mathf.Sin(angle);

      Debug.Log("x: " + x + " z: " + z);

      return x;
    }



    /* OnCollisionEnter
      => Destroy obj when collision occures
    */
    void OnCollisionEnter(Collision col){

      Destroy(gameObject);
    }
}
