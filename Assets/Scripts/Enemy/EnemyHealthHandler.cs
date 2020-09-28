using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Determine if health is below zero
        if (health <= 0){
          Destroy(gameObject);
        }
    }


    /* HitDamage

    */
    public void HitDamage(int damage){
      health -= damage;
    }
}
