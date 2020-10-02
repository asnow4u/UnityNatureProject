using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyController : MonoBehaviour
{

  public int health;

  private Animator animator;

  // Start is called before the first frame update
  void Start(){

    animator = GetComponent<Animator>();
  }


  /*  UPDATE
    => Update Movement each frame
    => Determine if crossing center area

    //TODO LIST:
      -Attacking player if in range
      -Jumping if nessisary
  */
  void Update(){

    //Determine if health is below zero
    if (health <= 0){
      animator.SetTrigger("Dead");
      Destroy(gameObject);

    } else {

      UpdateMovement();
    }
  }


  /* HitDamage

  */
  public void HitDamage(int damage){
    animator.SetTrigger("DamageTaken");
    health -= damage;
  }
}
