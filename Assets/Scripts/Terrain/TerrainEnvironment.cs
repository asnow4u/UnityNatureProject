using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TerrainMain : MonoBehaviour
{
    public GameObject smallTree;
    public GameObject smallRock;

    private struct Enviroment {
      public List<GameObject> smallTrees;
      public List<GameObject> rocks;
    }

    /* EnvironmentPlacement
      => Update enviroment within a selected tile
    */
    void EnvironmentPlacement(int i){

      //Remove current enviroment
      RemoveEnviroment(i);

      //Update player area to edge of hill
      UpdateForground( FindAngle(i), i );

      //Update hill area to path
      UpdateMidground( FindAngle(i), i );

      //Update beyond the path
      UpdateBackground( FindAngle(i), i );

    }


    /* FindAngle
      => Return a random angle within a selected tile
    */
    private float FindAngle(int i){
      float FLmagnitude;
      float FRmagnitude;
      float terrainAngle;
      float sign;

      //Get the vectors for FL/FR of the terrain tile and an angle reference point (terrainTile 0)
      Vector3 refPoint = new Vector3(vertices[terrainTiles[0].farLeft[0]].x, vertices[terrainTiles[0].farLeft[0]].y, 0);
      Vector3 FLVector = new Vector3(vertices[terrainTiles[i].farLeft[0]].x, vertices[terrainTiles[i].farLeft[0]].y, 0);
      Vector3 FRVector = new Vector3(vertices[terrainTiles[i].farRight[0]].x, vertices[terrainTiles[i].farRight[0]].y, 0);

      //Establish angles for each terrain
      // 0: 180, 1: 135, 2: 90, 3: 45, 4: 0, 5: 315, 6: 270, 7: 225 (difference of 45)
      terrainAngle = Vector3.Angle(refPoint, FLVector);
      sign = Mathf.Sign(Vector3.Dot(new Vector3(0, 0, 1), Vector3.Cross(refPoint, FLVector)));

      terrainAngle = terrainAngle * sign;
      terrainAngle = (terrainAngle + 180) % 360;

      //Pick Random angle within range
      float randAngle = Random.Range(0.0f, 45.0f);
      terrainAngle -= randAngle;

      return terrainAngle;
    }


    /* UpdateForground
      => Place objects between 5 to 11 from the center point
    */
    private void UpdateForground(float angle, int i){

      //Pick Random distance from center point
      float dist = Random.Range(5.0f, 11.0f);

      //Get x and y position
      Vector2 pos = new Vector2();
      pos = GetEnviromentLocation(angle, dist, i);

      float z = -1.0f;

      //TODO: randomize obj to be spawned

      //Instantiate GameObject
      GameObject obj = Instantiate(smallTree, new Vector3(pos.y, z, pos.x), Quaternion.identity);
      terrainTiles[i].enviroment.smallTrees.Add(obj);

      // Rotate Tree on local axis
      float rotationAngle = Random.Range(0f, 360f);
      obj.transform.RotateAround(obj.transform.position, obj.transform.up, rotationAngle);

      //Randomly adjust scaling from 0.8x to 2x
      float randomScale = Random.Range(0.8f, 2.0f);
      obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(randomScale, randomScale, randomScale));

      //TODO: After scalling move down till bottom of object is covered
      //TODO: use raycasting to get the normal of the terrain
    }


    /* UpdateMidground
      => Place objects between 12 to 18 from the center point
    */
    private void UpdateMidground(float angle, int i){

      //Pick Random distance from center point
      float dist = Random.Range(12.0f, 18.0f);

      //Get x and y position
      Vector2 pos = new Vector2();
      pos = GetEnviromentLocation(angle, dist, i);

      //TODO: randomize obj to be spawned

      //Instantiate GameObject
      GameObject obj = Instantiate(smallTree, new Vector3(pos.y, 0, pos.x), Quaternion.identity);
      terrainTiles[i].enviroment.smallTrees.Add(obj);

      // Rotate Tree on local axis
      float rotationAngle = Random.Range(0f, 360f);
      obj.transform.RotateAround(obj.transform.position, obj.transform.up, rotationAngle);

      //Randomly adjust scaling from 0.8x to 2x
      float randomScale = Random.Range(0.8f, 2.0f);
      obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(randomScale, randomScale, randomScale));

      //TODO: After scalling move down till bottom of object is covered
      //TODO: Future: Could form some trees that come out at an angle (using the normal of the position)

    }


    /* UpdateBackground
      => Place objects between 22 to 25 from the center point

    */
    private void UpdateBackground(float angle, int i){

      //Pick Random distance from center point
      float dist = Random.Range(22.0f, 25.0f);

      //Get x and y position
      Vector2 pos = new Vector2();
      pos = GetEnviromentLocation(angle, dist, i);


      //TODO: randomize obj to be spawned

      //Instantiate GameObject
      GameObject obj = Instantiate(smallTree, new Vector3(pos.y, 0, pos.x), Quaternion.identity);
      terrainTiles[i].enviroment.smallTrees.Add(obj);

      // Rotate Tree on local axis
      float rotationAngle = Random.Range(0f, 360f);
      obj.transform.RotateAround(obj.transform.position, obj.transform.up, rotationAngle);

      //Randomly adjust scaling from 0.8x to 2x
      float randomScale = Random.Range(0.8f, 2.0f);
      obj.transform.localScale = Vector3.Scale(obj.transform.localScale, new Vector3(randomScale, randomScale, randomScale));

      //TODO: After scalling move object to height of terrain
      //TODO: Use raycast to get normal for placing tree
    }


    /* CheckEnviromentLocation
      => Test the x and y of previous enviroment objs to prevent spawning on same space
    */
    private Vector2 GetEnviromentLocation(float angle, float dist, int i){

      float x = dist * Mathf.Cos(angle * Mathf.Deg2Rad);
      float y = dist * Mathf.Sin(angle * Mathf.Deg2Rad);

      foreach (GameObject obj in terrainTiles[i].enviroment.smallTrees) {
        if (obj.transform.position.x == x && obj.transform.position.y == y){ //Change this to distance
          return GetEnviromentLocation(angle, dist, i);
        }
      }

      Vector2 pos = new Vector2(x, y);
      return pos;
    }


    /* RemoveEnviroment
      => Remove the enviroment from the selected tile
    */
    private void RemoveEnviroment(int i){

      foreach (GameObject obj in terrainTiles[i].enviroment.smallTrees) {

        Destroy(obj);
      }

      terrainTiles[i].enviroment.smallTrees.Clear();
    }


    //TODO: move to EnemySpawn.cs
    /* PlaceEnemy
      => Randomly determines where an enemy should be placed based on tile vertices
      => Calls function from EnemySpawn, SpawnEnemy() to determine if / which enemy should be placed
    */
    //TODO: When getting the vertex on where to place enemy, determine the x and z so its deffinitly in the middle
    void PlaceEnemy(int quadNum){

      int randNum = Random.Range(1, 4);
      Vector3 v = new Vector3();

      switch(randNum){
        case 1:
          v = transform.TransformPoint(vertices[terrainTiles[quadNum].farLeft[0]].x * 1.1f, vertices[terrainTiles[quadNum].farLeft[0]].y, vertices[terrainTiles[quadNum].farLeft[0]].z * 1.1f);
          enemySpawn.SpawnEnemy(v, player.GetComponent<GardianController>().GetProgression());
          break;

        case 2:
          v = transform.TransformPoint(vertices[terrainTiles[quadNum].midLeft[0]].x * 1.1f, vertices[terrainTiles[quadNum].midLeft[0]].y, vertices[terrainTiles[quadNum].midLeft[0]].z * 1.1f);
          enemySpawn.SpawnEnemy(v, player.GetComponent<GardianController>().GetProgression());
          break;

        case 3:
          v = transform.TransformPoint(vertices[terrainTiles[quadNum].midRight[0]].x * 1.1f, vertices[terrainTiles[quadNum].midRight[0]].y, vertices[terrainTiles[quadNum].midRight[0]].z * 1.1f);
          enemySpawn.SpawnEnemy(v, player.GetComponent<GardianController>().GetProgression());
          break;

        case 4:
          v = transform.TransformPoint(vertices[terrainTiles[quadNum].farRight[0]].x * 1.1f, vertices[terrainTiles[quadNum].farRight[0]].y, vertices[terrainTiles[quadNum].farRight[0]].z * 1.1f);
          enemySpawn.SpawnEnemy(v, player.GetComponent<GardianController>().GetProgression());
          break;
      }
    }

}
