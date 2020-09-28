using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyObj;
    private List<GameObject> enemyList;
    private int enemyHeight;
    //TODO will need to create an array of all the enemies spawed


    /* Start
      => Initialize Variables
    */
    void Start()
    {
      enemyList = new List<GameObject>();
    }


    /* Update

    */
    void Update()
    {

    }


    /* SpawnEnemy

    */
    public void SpawnEnemy(Vector3 pos, float playerDistance){

      //NOTE: Player distance can help determine which enemy to spawn
      /*TODO
        -randomize if an enemy should be spawned
        -randomize if a enemy will be placed based on number of enemys
        -determine which enemy will be placed based on number of different enemys and player distance traveled
      */

      if (enemyList.Count < 2){

        enemyHeight = 5; //TODO: adjust for enemy height

        enemyList.Add(Instantiate(enemyObj, new Vector3(pos.x, pos.y + enemyHeight, pos.z), Quaternion.identity));

        //TODO: need to fix the z axis based on where we spawn
        //Spawn enemy facing the proper direction
        enemyList[enemyList.Count -1].transform.Rotate(-90f, 180f, 0f, Space.Self);

      }
    }


    /* GetNumEnemies
      => Return current number of enemies
    */
    public int GetNumEnemies(){
      return enemyList.Count;
    }


    public Vector3 GetEnemyPosition(int i){
      return enemyList[i].transform.position;
    }
}
