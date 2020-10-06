using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyController : MonoBehaviour
{

  public float attackRange;
  public float attackHeight;
  public bool attacking;
  public LayerMask gardianLayer;


  /* CheckAttackRange

  */
  private void CheckAttackRange(){

    Collider[] colGardian = Physics.OverlapSphere(transform.position + transform.up * attackHeight + transform.forward * attackRange, 1.5f, gardianLayer);
    foreach (Collider col in colGardian){


      attacking = true;
      animator.SetTrigger("Attack");

      Invoke("Attack", 1.5f);

      Debug.Log("attack player");

    }
  }


  /* Attacking

  */
  private void Attack(){
    Collider[] colGardian = Physics.OverlapSphere(transform.position + transform.up * attackHeight + transform.forward * attackRange, 1.5f, gardianLayer);
    foreach (Collider col in colGardian){

      //TODO: Deal damage to player
      //col.GetComponent<GardianController>().takeDamage();

      Debug.Log("deal damage");

    }
    
    attacking = false;
  }


  void OnDrawGizmosSelected(){
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position + transform.up * attackHeight + transform.forward * attackRange, 1.5f);
  }
}
