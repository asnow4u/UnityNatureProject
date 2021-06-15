using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerHandler : MonoBehaviour
{

    private enum power { water, heal, thorn, red } //TODO: need 1 or 2 more powers
    private power curPower;
    private bool overAim;

    [SerializeField]
    private GameObject waterSymbolPrefab, healSymbolPrefab, thornSymbolPrefab, otherSymbolPrefab;

    [SerializeField]
    public GameObject actionSelectorPrefab;
    private GameObject actionSelector;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private GameObject powerReticle;

    [SerializeField]
    private GameObject healPower;

    [SerializeField]
    private GameObject waterPower;

    [SerializeField]
    private GameObject thornPower;

    [SerializeField]
    private int healTimer;
    private float healCooldown;

    [SerializeField]
    private int waterTimer;
    private float waterCooldown;

    [SerializeField]
    private int thronTimer;
    private float thornCooldown;

    private GameObject[] activePowerSymbols;
    private GameObject[] selectPowerSymbols;


    // Start is called before the first frame update
    void Start()
    {
      curPower = power.heal;

      healCooldown = 0;
      waterCooldown = 0;
      thornCooldown = 0;

      overAim = false;

      powerReticle = Instantiate(powerReticle, new Vector3(0,0,0), Quaternion.Euler(-90, 0, 0));
      powerReticle.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
      if (healCooldown > 0) {
        healCooldown -= Time.deltaTime;
      }
    }


    public void ActivatePower(Vector3 handPos, Vector3 forwardDirection) {

      if ( (curPower == power.heal && healCooldown <= 0) || (curPower == power.water && waterCooldown <= 0) || (curPower == power.thorn && thornCooldown <= 0) ) {

        RaycastHit hit;

        if (Physics.Raycast(handPos, forwardDirection, out hit, Mathf.Infinity, groundLayer)) {

          if (powerReticle.activeSelf) {
            powerReticle.transform.position = hit.point;
          }

          else {
            powerReticle.transform.position = hit.point;
            toggleReticle(true);
          }

        //Player is pointing upward
        } else {

          //Ray direction from controller
          Vector3 dir = Vector3.Scale(handPos, forwardDirection);

          //Get angle
          float x = Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z);
          float angle = Mathf.Rad2Deg * Mathf.Atan(dir.y / x);

          //Only work for angles below a threshold
          if (angle <= 40f) {

            if (overAim) {
              overAim = false;
              toggleReticle(true);
            }

            Vector3 point = new Vector3();
            point = new Vector3(forwardDirection.x, 0, forwardDirection.z) * 80;

            powerReticle.transform.position = point;

            //TODO: get height from the tile section and adjust to that height

          } else {

            overAim = true;
            toggleReticle(false);
          }
        }
      }
    }


    public void ShootPower() {

      if (curPower == power.heal) {

        Instantiate(healPower, powerReticle.transform.position, Quaternion.Euler(-90, 0, 0));
        healCooldown = healTimer;
        toggleReticle(false);
      }

      else if (curPower == power.water) {

      }

      else if (curPower == power.thorn) {

      }


    }


    public void toggleReticle(bool status) {

      if (status) {
        powerReticle.SetActive(true);

      } else {
        powerReticle.SetActive(false);
      }
    }
}
