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
    movingClockWise = true;
  }


  /*  UPDATE
    => Update Movement each frame
    => Determine if crossing center area
  */
  void Update(){

  //Determine if health is below zero
    if (!attacking && health > 0){

      UpdateMovement();
      CheckAttackRange();
    }
  }


  /* HitDamage

  */
  public void HitDamage(int damage){

    health -= damage;

    if (health <= 0){
      animator.SetTrigger("Dead");
      Invoke("Dying", 1.3f);

    } else {
      animator.SetTrigger("DamageTaken");
    }
  }


  /* Dying

  */
  private void Dying(){
    Destroy(gameObject);
  }


}
