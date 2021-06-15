using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private GameObject localAvatar;
    private GameObject rightHand;
    private GameObject leftHand;

    private Vector2 moveStick;
    private Vector2 aimStick;

    private bool powerActive;

    private enum hand{right, left}
    private hand dominateHand;

    [SerializeField]
    private GameObject gardian;

    private GardianMovementHandler movementHandler;
    private GardianAttackHandler attackHandler;
    private PlayerPowerHandler powerHandler;

    void Awake() {
      movementHandler = gardian.GetComponent<GardianMovementHandler>();
      attackHandler = gardian.GetComponent<GardianAttackHandler>();
      powerHandler = GetComponent<PlayerPowerHandler>();

    }

    /* Start
      => Initialize Variables
    */
    void Start()
    {

      powerActive = false;

      //TODO get dominateHand from options menu
      dominateHand = hand.right;

    }


    /* Update

    */
    void Update(){

      // Check that both right and left hand objects are established
      if (rightHand == null){
        if (localAvatar.transform.Find("hand_right") != null){
          rightHand = localAvatar.transform.Find("hand_right").gameObject;

          if (dominateHand == hand.right){

            // InitiatePowerLayout(rightHand);

          }
        }
      }

      if (leftHand == null){
        if (localAvatar.transform.Find("hand_left") != null){
          leftHand = localAvatar.transform.Find("hand_left").gameObject;

          if (dominateHand == hand.left){

            // InitiatePowerLayout(leftHand);
          }
        }
      }


      // Dominate Right Hand
      if (dominateHand == hand.right){

        DominateRightControllerInputs();

      // Dominate Left Hand
      } else {

        DominateLeftControllerInputs();
      }

    }

    //FixedUpdate
    void FixedUpdate() {

      if (powerActive) {
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger) && (!OVRInput.Get(OVRInput.RawButton.LHandTrigger))) {

          powerHandler.ActivatePower(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), leftHand.transform.forward);
        }

        else if (OVRInput.Get(OVRInput.RawButton.LHandTrigger) && (!OVRInput.Get(OVRInput.RawButton.RHandTrigger))) {

          powerHandler.ActivatePower(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), rightHand.transform.forward);
        }
      }
    }


    /* DominateRightControllerInputs
      => moveStick to move player around based on x
      => Trigger for jump
    */
    private void DominateRightControllerInputs() {

      //Movement
      moveStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
      movementHandler.UpdateMovement(moveStick.x);

      //Aiming
      aimStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);
      attackHandler.UpdateAim(aimStick);

      // Jumping
      if (OVRInput.GetDown(OVRInput.RawButton.A)) {
        movementHandler.Jump();
      }

      // Attack
      if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) {
        attackHandler.UpdateAttackAnimation();
      }

      // Ability (Dash)
      if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)) {
        movementHandler.Dash(aimStick);
      }

      //Activate Power
      if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)) {

        if (powerActive) {
          powerHandler.ShootPower();
        }

        else {
          powerActive = true;
        }
      }

      if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)) {

        if (powerActive) {
          powerHandler.ShootPower();
        }

        else {
          powerActive = true;
        }
      }

      if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) || OVRInput.GetUp(OVRInput.RawButton.LHandTrigger)) {

        //Determine if both triggers are up
        if ((!OVRInput.Get(OVRInput.RawButton.RHandTrigger)) && (!OVRInput.Get(OVRInput.RawButton.LHandTrigger))) {
          powerActive = false;
          powerHandler.toggleReticle(false);
        }
      }


      // Action Trigger
      // if (OVRInput.Get(OVRInput.RawButton.RHandTrigger)){
      //
      //   switch(curAction){
      //     case action.water:
      //       Debug.Log("water");
      //       WaterAction(rightHandOrigin);
      //       break;
      //
      //     case action.thorns:
      //       Debug.Log("Thorns");
      //       ThornAction(rightHandOrigin, rightHand, OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger));
      //       break;
      //
      //     case action.green:
      //       Debug.Log("Regrowth");
      //       RegrowthAction(rightHandOrigin, rightHand, OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger));
      //       break;
      //
      //     case action.red:
      //       Debug.Log("Attack");
      //       AttackAction(rightHandOrigin);
      //       break;
      //   }
      //
      // } else {
      //
      //   //Set lineRenderer so its hidden
      //   lineRenderer.SetPosition(0, rightHandOrigin);
      //   lineRenderer.SetPosition(1, rightHandOrigin);
      //
      //   //Check for and destroy any remaining children
      //   if (transform.childCount > 0){
      //     for (int i=0; i<transform.childCount; i++){
      //       GameObject.Destroy(transform.GetChild(i).gameObject);
      //     }
      //   }
      // }
    }



    //TODO: update based on DominateRightController (just the opposite)
    /* DominateLeftControllerInputs

    */
    private void DominateLeftControllerInputs(){

      //Movement
      moveStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);
      movementHandler.UpdateMovement(moveStick.x);


      // Jumping
      if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)){
        movementHandler.Jump();
      }

      //Get current action
      //UpdateCurrentAction(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch));
    }


    /* InitiatePowerLayout

      0 = water
      1 = heal
      2 = other
      3 = thorn
    */
    private void InitiatePowerLayout(GameObject playerHand) {

      // //Instantiate all the symbols that will indicate the active power
      // activePowerSymbols[0] = Instantiate(waterSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // activePowerSymbols[1] = Instantiate(healSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // activePowerSymbols[2] = Instantiate(otherSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // activePowerSymbols[3] = Instantiate(thornSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      //
      // //Set parent, rotation, and position, and hide them from the player to be reveled when needed
      // for (int i=0; i<4; i++){
      //   activePowerSymbols[i].transform.parent = playerHand.transform;
      //   activePowerSymbols[i].transform.Rotate(-90.0f, 180f, 0f, Space.Self);
      //   activePowerSymbols[i].transform.position = new Vector3(activePowerSymbols[i].transform.position.x, activePowerSymbols[i].transform.position.y + 0.03f, activePowerSymbols[i].transform.position.z);
      //   activePowerSymbols[i].SetActive(false);
      // }
      //
      // //Instantiate all the symbols that will be used when selecting a new power
      // selectPowerSymbols[0] = Instantiate(waterSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // selectPowerSymbols[1] = Instantiate(healSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // selectPowerSymbols[2] = Instantiate(otherSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      // selectPowerSymbols[3] = Instantiate(thornSymbolPrefab, playerHand.transform.position, Quaternion.identity);
      //
      // for (int i=0; i<4; i++){
      //   selectPowerSymbols[i].transform.parent = playerHand.transform;
      //   selectPowerSymbols[i].transform.Rotate(-90.0f, 180f, 0f, Space.Self);
      //   selectPowerSymbols[i].SetActive(false);
      // }
      //
      // selectPowerSymbols[0].transform.position = new Vector3(selectPowerSymbols[0].transform.position.x + 0.11f, selectPowerSymbols[0].transform.position.y + 0.03f, selectPowerSymbols[0].transform.position.z);
      // selectPowerSymbols[1].transform.position = new Vector3(selectPowerSymbols[1].transform.position.x, selectPowerSymbols[1].transform.position.y + 0.03f, selectPowerSymbols[1].transform.position.z + 0.11f);
      // selectPowerSymbols[2].transform.position = new Vector3(selectPowerSymbols[2].transform.position.x, selectPowerSymbols[2].transform.position.y + 0.03f, selectPowerSymbols[2].transform.position.z - 0.11f);
      // selectPowerSymbols[3].transform.position = new Vector3(selectPowerSymbols[3].transform.position.x - 0.11f, selectPowerSymbols[3].transform.position.y + 0.03f, selectPowerSymbols[3].transform.position.z);
      //
      // //Instantiate powerSelector
      // actionSelector = Instantiate(actionSelectorPrefab, new Vector3(playerHand.transform.position.x, playerHand.transform.position.y + 0.03f, playerHand.transform.position.z), Quaternion.identity);
      // actionSelector.transform.parent = playerHand.transform;
      // actionSelector.transform.Rotate(-90.0f, 180f, 0f, Space.Self);
      // actionSelector.SetActive(false);
    }


    /* UpdateCurrentAction
      => Check to see if current action needs to be changed
      => Based on the location of the stick, apply the appropriate action
    */
    private void UpdateCurrentAction(Vector2 actionStick){

      // //Right
      // if (actionStick.x >= 0.5f && actionStick.y < 0.5f && actionStick.y > -0.5f){
      //   curAction = action.water;
      //   attackHandler.UpdateCurrentProjectile(true, false, false, false);
      //
      // //Left
      // } else if (actionStick.x <= -0.5f && actionStick.y < 0.5f && actionStick.y > -0.5f){
      //   curAction = action.thorns;
      //   attackHandler.UpdateCurrentProjectile(false, true, false, false);
      //
      // //Up
      // } else if (actionStick.x < 0.5f && actionStick.x > -0.5f && actionStick.y >= 0.5f){
      //   curAction = action.red;
      //   attackHandler.UpdateCurrentProjectile(false, false, true, false);
      //
      // //Down
      // } else if (actionStick.x < 0.5f && actionStick.x > -0.5f && actionStick.y <= -0.5f){
      //   curAction = action.yellow;
      //   attackHandler.UpdateCurrentProjectile(false, false, false, true);
      //
      // }
      //
      // //Reveil power wheel and hide active power
      // if (actionStick.x > 0.5f || actionStick.x < -0.5f || actionStick.y > 0.5f || actionStick.y < -0.5f){
      //   for (int i=0; i<4; i++){
      //     activePowerSymbols[i].SetActive(false);
      //     selectPowerSymbols[i].SetActive(true);
      //   }
      //
      //   //Reveil actionSelector
      //   actionSelector.SetActive(true);
      //
      //   //Rotate actionSelector to where the analog stick points
      //   float angle = Mathf.Atan2(actionStick.y, actionStick.x) * Mathf.Rad2Deg;
      //   actionSelector.transform.localRotation = Quaternion.Euler(0, (-1f) * angle, 0);
      //   actionSelector.transform.Rotate(-90.0f, 225f, 0f, Space.Self);
      //
      // } else {
      //
      //   //Hide power wheel
      //   for (int i=0; i<4; i++){
      //     selectPowerSymbols[i].SetActive(false);
      //   }
      //
      //   //Hide actionSelector
      //   actionSelector.SetActive(false);
      //
      //
      //   switch (curAction){
      //     case action.red:
      //       activePowerSymbols[1].SetActive(true);
      //       break;
      //     case action.water:
      //       activePowerSymbols[0].SetActive(true);
      //       break;
      //     case action.thorns:
      //       activePowerSymbols[3].SetActive(true);
      //       break;
      //     case action.yellow:
      //       activePowerSymbols[2].SetActive(true);
      //       break;
      //   }
      // }
    }


    /* WaterAction

    */
    private void WaterAction(Vector3 origin){

    }


    /* ThornAction

    */
    private void ThornAction(Vector3 origin, GameObject hand, bool triggerPressed){

      // RaycastHit hit;
      // GameObject reticle;
      //
      // if (Physics.Raycast(origin, hand.transform.forward, out hit, Mathf.Infinity)){
      //
      //   if (transform.childCount > 0){
      //
      //     //Loop through to see if a reticle obj exist as a child
      //     for (int i=0; i<transform.childCount; i++){
      //
      //       reticle = transform.GetChild(i).gameObject;
      //
      //       //Update position
      //       if (reticle.tag == "Reticle"){
      //         reticle.transform.position = hit.point;
      //         break;
      //       }
      //     }
      //
      //   //Spawn reticle
      //   } else {
      //     reticle = Instantiate(thornTargetArea, hit.point, Quaternion.identity);
      //     reticle.transform.parent = transform;
      //   }
      //
      // //Find position due to user pointing up
      // } else {
      //
      //   if (transform.childCount > 0){
      //     for (int i=0; i<transform.childCount; i++){
      //       reticle = transform.GetChild(i).gameObject;
      //
      //       if (reticle.tag == "Reticle"){
      //         reticle.transform.position = GetPathLocation(hand, origin);
      //       }
      //     }
      //
      //   } else {
      //     reticle = Instantiate(thornTargetArea, GetPathLocation(hand, origin), Quaternion.identity);
      //     reticle.transform.parent = transform;
      //   }
      // }
      //
      // if (triggerPressed){
      //   if (transform.childCount > 0){
      //     for (int i=0; i<transform.childCount; i++){
      //       reticle = transform.GetChild(i).gameObject;
      //       if (reticle.tag == "Reticle"){
      //         reticle.transform.parent = null;
      //         Instantiate(thorns, reticle.transform.position, Quaternion.identity);
      //         Destroy(reticle);
      //         //TODO: start cooldown(prevent multiple from being spawned)
      //         //TODO: need to adjust thorns position
      //
      //       }
      //     }
      //   }
      // }
    }


    /* RegrowthAction

    */
    private void RegrowthAction(Vector3 origin, GameObject hand, bool triggerPressed){

      // RaycastHit hit;
      // float lineDist = 20.0f;
      //
      // origin += hand.transform.forward * 0.05f;
      //
      // //Raycast to determine collision with other objects
      // if (Physics.Raycast(origin, hand.transform.forward, out hit, lineDist)){
      //
      //   //Not sure if Im gonna use this
      //   if (hit.transform.gameObject.tag == "Player"){
      //     lineRenderer.material.color = Color.green;
      //   }
      //
      //   lineDist = hit.distance;
      //
      // } else {
      //   lineDist = 20.0f;
      // }
      //
      // //Set line position
      // lineRenderer.SetPosition(0, origin);
      // lineRenderer.SetPosition(1, origin + hand.transform.forward * lineDist);
      //
      // //Check if PrimaryIndexTrigger is pressed
      // if (triggerPressed){
      //   Debug.Log("projectile spawn");
      // }

    }


    /* AttackAction

    */
    private void AttackAction(Vector3 origin){

    }


    /* GetPathLocation
      => Using the direction and height angle, this determins a point that is 20f away from the center
    */
    private Vector3 GetPathLocation(GameObject hand, Vector3 origin){

      //Ray direction from controller
      Vector3 dir = Vector3.Scale(origin, hand.transform.forward);

      //Get angle
      float x = Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z);
      float angle = Mathf.Rad2Deg * Mathf.Atan(dir.y / x);

      // Only work for angles below a threshold
      if (angle <= 40f){

        //Determine point that is above the path
        float dist = 20f / (Mathf.Cos((angle * Mathf.PI) / 180f));
        Vector3 point = new Vector3();
        point = origin + hand.transform.forward * dist;

        //Raycast to determine point on path
        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, Mathf.Infinity)){

          //TODO: need to determine if point is part of another object beside the gound
          return hit.point;
        }

        return point;

      //Angle to high, place reticle where it cant be seen
      } else {
        return new Vector3(0, -20, 0);
      }
    }
}
