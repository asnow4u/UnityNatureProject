using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GardianController : MonoBehaviour
{

  /* UseItem
    => Check that a tree is swingable
  */
  public void UseItem(){

    //TODO: will need to determine which item is equipped

    animator.SetTrigger("useItem");

    UseSwingItem();
  }

}
