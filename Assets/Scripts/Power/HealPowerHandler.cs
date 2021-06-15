using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerHandler : MonoBehaviour
{

    [SerializeField]
    private int despawnCounter;

    // Start is called before the first frame update
    void Start()
    {
      Destroy(gameObject, despawnCounter);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
