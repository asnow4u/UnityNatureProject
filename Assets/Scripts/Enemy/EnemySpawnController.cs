using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject gruntEnemy;
    //TODO have more enemy objs

    public List<GameObject> enemyList;
    public int maxChallengeLevel;
    public int curChallengeLevel;

    private TerrainMain terrainObj;

    /* Start
      => Initialize Variables
    */
    void Start()
    {
      terrainObj = GetComponent<TerrainMain>();

      enemyList = new List<GameObject>();

    }


    /* Update

    */
    void Update()
    {

    }


    //TODO: need to get gardians progression to better determine which enemies to spawn (later)
    /* UpdateEnemySpawn

    */
    public void UpdateEnemySpawn(int tileNum){

      //Determine where the enemy will spawn
      Vector3[] spawnPos = terrainObj.GetCenterPointsFromTile(tileNum);
      int rand = Random.Range(0, 3);

      //TODO:Determine which enemy/enemies is to be spawned based on progretion and the challengeLevel
      //TODO:Determine if group spawn or wave spawn

      //NOTE: Right now well focus on only spawning the gurnts
      if (curChallengeLevel < maxChallengeLevel){
        SpawnEnemy( spawnPos[rand], gruntEnemy);
      }
    }


    /* SpawnEnemy

    */
    public void SpawnEnemy(Vector3 pos, GameObject enemy){

      //Spawn enemy and add to list
      //NOTE: is having an enemylist needed?
      enemyList.Add(Instantiate(enemy, new Vector3(pos.x, pos.y + 0.01f, pos.z), Quaternion.identity));
      

      curChallengeLevel += 1;
    }


    /* GetNumEnemies
      => Return current number of enemies
    */
    public int GetNumEnemies(){
      return enemyList.Count;
    }


    /* UpdateCurChallengeLevel
      => Make changes to the current challenge level
      => This can occure when an enemy is destroyed or spawned
    */
    public void UpdateCurChallengeLevel(int level){
      curChallengeLevel += level;
    }
}
