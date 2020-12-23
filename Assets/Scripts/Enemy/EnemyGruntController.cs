using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGruntController : MonoBehaviour
{
    //General
    public int health;
    public float patrolMoveSpeed;
    public float forestMoveSpeed;
    public float chaseMoveSpeed;
    public enum Direction {cw, ccw, forest};
    public Direction direction;

    //Game Objects
    private Animator animator;
    public GameObject gruntFollower;
    public GameObject guardian;
    public GameObject terrainObj;
    public List<GameObject> gruntFollowers;
    public LayerMask guardianLayer;
    public LayerMask groundLayer;

    //States
    private struct State {
      public string name;
      public float moveSpeed;
      public float time;
    }

    private State idleState;
    private State patrolState;
    private State attackState;
    private State forestState;
    private State searchState;
    private State curState;
    public float chaseDistance;
    public bool searching;
    public bool summonedFollowers;
    private Vector3 forestCrossingPoint;

    //Attack
    public bool attack;
    public bool jumpAttack;
    public float attackRange;
    public float attackHeight;
    public float jumpForce;


    /* Start
      => Initilize variables and set states
    */
    void Start()
    {

      //Game Objects
      animator = GetComponent<Animator>();
      gruntFollowers = new List<GameObject>();
      guardian = GameObject.Find("Gardian");
      terrainObj = GameObject.Find("Terrain-Main");

      //Formate States
      idleState = new State();
      idleState.name = "idle";
      idleState.moveSpeed = 0f;
      idleState.time = Time.time;

      patrolState = new State();
      patrolState.name = "patrol";
      patrolState.moveSpeed = patrolMoveSpeed;

      attackState = new State();
      attackState.name = "attack";
      attackState.moveSpeed = chaseMoveSpeed;

      forestState = new State();
      forestState.name = "forest";
      forestState.moveSpeed = forestMoveSpeed;

      searchState = new State();
      searchState.name = "search";
      searchState.moveSpeed = chaseMoveSpeed;

      //Set Current State
      curState = idleState;
      direction = Direction.forest;

      searching = false;
      summonedFollowers = false;

      //Look towards the center
      transform.LookAt(new Vector3(0, transform.position.y, 0));
    }


    void Update()
    {
      UpdateState();
      UpdateMovement();
    }


    /* UpdateState
      => Update state machine (usually based on a time interval)
      => Idle State:
        => Change to Patrol State when time interval is up
        => Change to Attack State if gardian is in sight

      => Patrol State:
        => Change to Idle state when time interval is up
        => change to Attack State if gardian is in sight

      => Attack State
        => Summon followers if not already done
        => Attack if gardian is in range
        => Change to Searching State when sight on gardian is lost

      => Forest Crossing State
        => Change to Idle State when destination is reached

      => Searching
        => Change to Idle State when time interval is up
    */
    private void UpdateState(){

      float angle = Vector3.Angle(transform.position, guardian.transform.position);
      float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.position, guardian.transform.position)));
      angle = (angle * sign + 360) % 360;

      /* Idle State */
      if (curState.name == "idle"){
        if (Time.time > curState.time){
          patrolState.time = Time.time + Random.Range(3f, 12f);
          curState = patrolState;
          UpdateDirection();
        }

        else if ((direction == Direction.cw && angle <= 60) || (direction == Direction.ccw && angle >= 320)){

          curState = attackState;
        }

        //TODO: Noise detected
        // else if ()
      }

      /* Patrol State */
      else if (curState.name == "patrol"){
        if (Time.time > curState.time || CheckTerrainCliff()){
          idleState.time = Time.time + Random.Range(1f, 4f);
          curState = idleState;
        }

        else if ((direction == Direction.cw && angle <= 60) || (direction == Direction.ccw && angle >= 320)){

          curState = attackState;
        }

        //TODO: Noise detected
        // else if ()
      }

      /* Attack State */
      else if (curState.name == "attack"){

        if (!summonedFollowers){
          //Invoke("SpawnGruntFollowers", 1.5f); //NOTE: commented out only for the time being
          // animator.SetTrigger("Taunt");
          curState.time = Time.time + 2f;
          summonedFollowers = true;
          Debug.Log("Step 1: Summon");
        }

        if (Time.time > curState.time){

          CheckAttackRange();

          if (Vector3.Distance(transform.position, new Vector3(guardian.transform.position.x, transform.position.y, guardian.transform.position.z)) > chaseDistance){
            searchState.time = Time.time + 3f;
            curState = searchState;
            Debug.Log("Searching");
          }

        } else {

          //NOTE: could do collision (damage to player) here

          if (jumpAttack){

            if (curState.moveSpeed > 0f){
              curState.moveSpeed -= 0.1f;
              Debug.Log("moveSpeed: " + curState.moveSpeed);
            }

          } else {
            curState.moveSpeed = 0f;
          }

        }
      }

      /* Forest Crossing State */
      else if (curState.name == "forest"){
        if (Vector3.Distance(new Vector3(0f, transform.position.y, 0f), transform.position) >= 80f && Vector3.Distance(forestCrossingPoint, transform.position) < 80f){
          idleState.time = Time.time + 1f;
          curState = idleState;
          transform.LookAt(Vector3.zero);
        }
      }

      /* Searching State */
      else if (curState.name == "search"){
        if (Time.time > curState.time || CheckTerrainCliff()){
          Debug.Log("Search is over");
          idleState.time = Time.time + 2f;
          curState = idleState;
        }
      }
    }


    /* UpdateDirection
      => Randomly determines a direction (cw, ccw, forest)
      => Rotate grunt as need be for cw and ccw
      => Determine if forest crossing is possible
      => Prevent forest crossing from occuring multiple times
    */
    private void UpdateDirection(){

      float rand = Random.Range(0, 5);

      //Go CW
      if (rand == 0 || rand == 1){

        if (direction == Direction.cw){
          return;
        }

        else if (direction == Direction.ccw){
          direction = Direction.cw;
          transform.Rotate(0f, 180f, 0f);
        }

        else if (direction == Direction.forest){
          direction = Direction.cw;
          transform.Rotate(0f, -90f, 0f);
        }
      }

      //Go CCW
      else if (rand == 2 || rand == 3){

        if (direction == Direction.ccw){
          return;
        }

        else if (direction == Direction.cw){
          direction = Direction.ccw;
          transform.Rotate(0f, 180f, 0f);
        }

        else if (direction == Direction.forest){
          direction = Direction.ccw;
          transform.Rotate(0f, 90f, 0f);
        }
      }

      //Travel Through forest
      else if (rand == 4){

        //Prevent multiple forest crosses
        if (direction == Direction.forest || curState.name == "search"){
          UpdateDirection();

        } else {

          if (!CalculateForestCrossing()){
            UpdateDirection();

          } else {
            direction = Direction.forest;
            curState = forestState;
          }
        }
      }
    }


    /* CalculateForestCrossing
      => Determine the two tiles to cross to (3 apart from gurnts current tile)
      => Determine the 3 travel points from the center points of the tiles verts
      => Determine which travel points are good (dont contain a cliff)
      => Randomly choose travel point and return true
    */
    private bool CalculateForestCrossing(){

      int curTileNum;
      int rightTile;
      int leftTile;

      Vector3[] rightTravelPoints;
      Vector3[] leftTravelPoints;
      List<Vector3> travelPoints = new List<Vector3>();

      //Find current tile
      curTileNum = terrainObj.GetComponent<TerrainMain>().GetCurrentQuadrent(transform.position);

      //Determine posible left and right tiles to cross to
      leftTile = curTileNum + 3;
      if (leftTile > 7){
        leftTile -= 8;
      }

      rightTile = curTileNum - 3;
      if (rightTile < 0){
        rightTile += 8;
      }

      //Determine if any have a cliff and determine centerpoints
      if (!terrainObj.GetComponent<TerrainMain>().CheckTileForCliff(leftTile)){
        Vector3[] points = terrainObj.GetComponent<TerrainMain>().GetCenterPointsFromTile(leftTile);

        for (int i=0; i<points.Length; i++){
          travelPoints.Add(points[i]);
        }
      }

      if (!terrainObj.GetComponent<TerrainMain>().CheckTileForCliff(rightTile)){
        Vector3[] points = terrainObj.GetComponent<TerrainMain>().GetCenterPointsFromTile(rightTile);

        for (int i=0; i<points.Length; i++){
          travelPoints.Add(points[i]);
        }
      }

      if (travelPoints.Count == 0){
        return false;
      }

      //randomly choose from one of the center points
      int rand = Random.Range(0, travelPoints.Count);
      forestCrossingPoint = travelPoints[rand];

      return true;
    }


    /* CheckTerrainCliff
      => Return true if cliff is in front of grunt
    */
    private bool CheckTerrainCliff(){
      if (Physics.Raycast(transform.position + transform.up * 2f, transform.forward, 5f, groundLayer)){
        Debug.Log("Cliff Detected");
        return true;
      }

      return false;
    }


    /* CheckAttackRange
      => Determine if guardian is in range for an attack
      => When True, a time interval is set for the duration of the attack
      => When False, continue pursuit and adjust direction when needed (UpdateAttackDirection())
    */
    private void CheckAttackRange(){

      Rigidbody rb = GetComponent<Rigidbody>();

      if (!attack && !jumpAttack){

        curState.moveSpeed = attackState.moveSpeed;

        float angle = Vector3.Angle(transform.position, guardian.transform.position);
        float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.position, guardian.transform.position)));
        angle = (angle * sign + 360) % 360;

        if ((angle <= 5f && direction == Direction.cw) ||(angle >= 355f && direction == Direction.ccw)){
          Debug.Log("Basic Attack");
          curState.moveSpeed = 0f; //maybe
          attack = true;
          curState.time = Time.time + 1f; //attack animation time
          animator.SetTrigger("Attack");
        }

        //NOTE: the grunt will still attack if the guardian is jumping over. Might need to check the y diff, but also might be a funny interation
        else if ((angle <= 10f && direction == Direction.cw) || (angle >= 350f && direction == Direction.ccw)){
          Debug.Log("Jump Attack");
          rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          curState.moveSpeed += 2f; //initial jump speed
          jumpAttack = true;
          curState.time = Time.time + 1f; //jump Attack animation time
          // animator.SetTrigger("JumpAttack");
        }

        //TODO: For some reason not working as intended
        //Check if colliding with wall
        else if (CheckTerrainCliff()){
          idleState.time = Time.time + 2f;
          curState = idleState;
        }

        else {
          UpdateAttackDirection();
        }
      }

      else {

        if (attack){
          attack = false;
          curState.time = Time.time + 1f; //Pause between attacks
        }

        else if (jumpAttack){
          jumpAttack = false;
          curState.time = Time.time + 3f; //Get up after attack animation
        }
      }
    }


    /* UpdateAttackDirection
      => Ratate grunt to face the direction of the gardian
    */
    private void UpdateAttackDirection(){

      float angle = Vector3.Angle(transform.position, guardian.transform.position);
      float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.position, guardian.transform.position)));
      angle = (angle * sign + 360) % 360;

      //clockWiseDirection
      if (angle <= 60f){
        if (direction == Direction.ccw){
          direction = Direction.cw;
          transform.Rotate(0f, 180f, 0f);

          //TODO: implement a time waiting requirment when direction changes
        }
      }

      //Counter clockWiseDirection
      else if (angle >= 320f){
        if (direction == Direction.cw){
          direction = Direction.ccw;
          transform.Rotate(0f, 180f, 0f);
        }
      }
    }


    /* UpdateMovement
      => Rotate around based on movement speed
      => Or continue travel through the forest, headed towards the designated travel point
    */
    private void UpdateMovement(){

      if (curState.name == "forest"){
        transform.position = Vector3.MoveTowards(transform.position, forestCrossingPoint, curState.moveSpeed * Time.deltaTime);
        transform.LookAt(forestCrossingPoint);
        GroundAngle();
      }

      else {

        //rotate around
        if (direction == Direction.cw){
          transform.RotateAround(Vector3.zero, new Vector3(0,1,0), curState.moveSpeed * Time.deltaTime);
          animator.SetFloat("MoveSpeed", curState.moveSpeed);
        }

        else if (direction == Direction.ccw){
          transform.RotateAround(Vector3.zero, new Vector3(0,1,0), -curState.moveSpeed * Time.deltaTime);
          animator.SetFloat("MoveSpeed", curState.moveSpeed);
        }
      }

      GroundAngle();
    }


    /* GroundAngle
      => Find the angle to rotate the enemy model based on terrain
      => Uses the normal of the enemy towards the ground to determine the nessisary adjustments
    */
    private void GroundAngle(){

      RaycastHit hit;
      float angle;

      if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f,  1 << LayerMask.NameToLayer("Ground"))){
        transform.rotation = Quaternion.FromToRotation (transform.up, hit.normal) * transform.rotation;
      }
    }


    /* SpawnGruntFollowers
      => Spawn 4 followers and set there leader as this grunt
    */
    private void SpawnGruntFollowers(){

      float angle = 5f * Mathf.Deg2Rad;
      float xPos = transform.position.x * Mathf.Cos(angle) - transform.position.z * Mathf.Sin(angle);
      float zPos = transform.position.x * Mathf.Sin(angle) + transform.position.z * Mathf.Cos(angle);
      gruntFollowers.Add(Instantiate(gruntFollower, new Vector3(xPos, transform.position.y + 0.01f, zPos), Quaternion.identity));

      angle = 10f * Mathf.Deg2Rad;
      xPos = transform.position.x * Mathf.Cos(angle) - transform.position.z * Mathf.Sin(angle);
      zPos = transform.position.x * Mathf.Sin(angle) + transform.position.z * Mathf.Cos(angle);
      gruntFollowers.Add(Instantiate(gruntFollower, new Vector3(xPos, transform.position.y + 0.01f, zPos), Quaternion.identity));

      angle = -5f * Mathf.Deg2Rad;
      xPos = transform.position.x * Mathf.Cos(angle) - transform.position.z * Mathf.Sin(angle);
      zPos = transform.position.x * Mathf.Sin(angle) + transform.position.z * Mathf.Cos(angle);
      gruntFollowers.Add(Instantiate(gruntFollower, new Vector3(xPos, transform.position.y + 0.01f, zPos), Quaternion.identity));

      angle = -10f * Mathf.Deg2Rad;
      xPos = transform.position.x * Mathf.Cos(angle) - transform.position.z * Mathf.Sin(angle);
      zPos = transform.position.x * Mathf.Sin(angle) + transform.position.z * Mathf.Cos(angle);
      gruntFollowers.Add(Instantiate(gruntFollower, new Vector3(xPos, transform.position.y + 0.01f, zPos), Quaternion.identity));

      foreach(GameObject follower in gruntFollowers){
        follower.GetComponent<EnemyGruntFollowerController>().DesignateLeader(gameObject);
      }
    }


    /* HitDamage

    */
    public void HitDamage(int damage){

      health -= damage;

      if (health <= 0){
        // animator.SetTrigger("Dead");
        Invoke("Dying", 1.3f);

      } else {
        // animator.SetTrigger("DamageTaken");
      }
    }


    /* Dying

    */
    private void Dying(){
      Destroy(gameObject);
    }


    void OnDrawGizmosSelected(){
      Gizmos.color = Color.blue;
      // Gizmos.DrawWireSphere(transform.position + transform.up * attackHeight + transform.forward * attackRange, 3f);
      // Gizmos.DrawWireSphere(transform.position + transform.up * attackHeight, 3f);
      //
      Gizmos.DrawRay(transform.position + transform.up * 2f, transform.forward * 5f);
    }
}
