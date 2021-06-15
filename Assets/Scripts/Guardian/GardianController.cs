using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class GardianController : MonoBehaviour
{
    private int health;
    public Animator animator;
    public Rigidbody rb;


    /* Start
      => Initialize Variables
    */
    void Awake()
    {
      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
      health = 100;
    }


    /* Update
      => Test GameOver Constraints (Health < 0)
    */
    void Update()
    {

      // Test if players health is at or below 0
      if (health <= 0){
        // Debug.Log("Dead!" + health);
      }
    }


    /* OnCollisionEnter
    */
    // void OnCollisionEnter(Collision col){
    //
    //   //When an enemy hits player
    //   if (col.gameObject.tag == "Enemy"){
    //
    //     //TODO: determine type of enemy to determine how much damage is dealt
    //     health -= 10;
    //
    //   }
    // }


    /* Getters and Setters
      => Get values from this class
      => Set values from this class
    */

    public int GardianHealth {
      get { return health; }
    }

}
