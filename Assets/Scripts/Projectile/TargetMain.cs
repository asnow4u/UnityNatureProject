using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMain : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
      // //Check that this is no longer attached to the player
      if (transform.parent == null){
        // if (transform.localScale.x < 5){
        //   transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z + 0.05f);
        // }
        
      }
    }
}
