using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TerrainMain : MonoBehaviour
{
    public Camera cam;
    private Mesh mesh;
    private Vector3[] vertices;

    private int curPlayerQuad;
    private bool updateTerrain;

    private float maxTerrainHeight;
    private float minTerrainHeight;

    private enum TerrainType {Uphill, Downhill, Flat, Pitfall, Wall, Cliff};

    private struct TerrainTile {
        public int height;
        public bool isExplored;
        public List<int> vertNum;
        public List<int> farRight;
        public List<int> midRight;
        public List<int> midLeft;
        public List<int> farLeft;
        public TerrainType type;
        public Enviroment enviroment;
    }
    private TerrainTile[] terrainTiles;

    private EnemySpawn enemySpawn;
    private GameObject player;

    /* Start
      => Initialize Variables
      => Set up mesh verts into specified terrainTiles
    */
    void Start()
    {

        updateTerrain = false;
        maxTerrainHeight = 0.1f;
        minTerrainHeight = 0.0f;

        enemySpawn = GetComponent<EnemySpawn>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Establish the 8 tiles
        terrainTiles = new TerrainTile[]{
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile(),
            new TerrainTile()
        };

        //Initialize tile values
        for (int i=0; i<terrainTiles.Length; i++){
          terrainTiles[i].height = 0;
          terrainTiles[i].isExplored = false;
          terrainTiles[i].vertNum = new List<int>();
          terrainTiles[i].farRight = new List<int>();
          terrainTiles[i].midRight = new List<int>();
          terrainTiles[i].midLeft = new List<int>();
          terrainTiles[i].farLeft = new List<int>();
          terrainTiles[i].type = TerrainType.Flat;
          terrainTiles[i].enviroment.smallTrees = new List<GameObject>();
          terrainTiles[i].enviroment.rocks = new List<GameObject>();

        }

        //Grab vertices from mesh
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        //Determine which vertices go to which quadrent
        for (int i=0; i<vertices.Length; i++){

          // Check that vertice is part of the path
          if (vertices[i].z > 0.01){

            Vector3 center = new Vector3(0.0f, 0.0f, vertices[i].z);
            float dist = Vector3.Distance(vertices[i], center);

            // Get the center point between the center and the vertex
            dist = Mathf.Sqrt((dist*dist)/2);
            dist = Mathf.Round(dist * 1000f) / 1000f;

            DetermineQuadrent(vertices[i], i, dist);
          }
        }

        DetermineVerticeGroups();

        //Testing Left
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].farRight[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].midRight[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].midLeft[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].farLeft[i]].z = 0.06f;
        // }
        //
        // //Testing Middle
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].farRight[i]].z = 0.02f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].midRight[i]].z = 0.02f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].midLeft[i]].z = 0.02f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[1].farLeft[i]].z = 0.02f;
        // }
        // //
        // //Testing Right
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[2].farRight[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[2].midRight[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[2].midLeft[i]].z = 0.06f;
        // }
        // for(int i=0; i<4; i++){
        //   vertices[terrainTiles[2].farLeft[i]].z = 0.06f;
        // }

        terrainTiles[0].isExplored = true;
        terrainTiles[1].isExplored = true;
        terrainTiles[2].isExplored = true;
        terrainTiles[3].isExplored = true;
        terrainTiles[4].isExplored = true;
        terrainTiles[5].isExplored = true;
        terrainTiles[6].isExplored = true;
        terrainTiles[7].isExplored = true;

    }


    /* Update
      => Update where player has explored

    */
    void Update()
    {

      //Update player explored quadrents
      if (player != null){

        curPlayerQuad = GetCurrentQuadrent(player.transform.position);

        if (curPlayerQuad >= 0){
          terrainTiles[curPlayerQuad].isExplored = true;
        }

      } else {
        player = GameObject.FindGameObjectWithTag("Player");
      }

      //Loop through each terrain quad
      for(int i=0; i<terrainTiles.Length; i++){

        //Test if terrain is explored by player and constraints
        if(terrainTiles[i].isExplored){
          if (TerrainConstraints(i)){

            //TODO: need to fix
            //Check that terrain is outside of left eye view
            // Vector3 vert = transform.TransformPoint(vertices[terrainTiles[i].farLeft[0]]);
            // Vector3 leftViewPos = cam.WorldToViewportPoint(vert);
            // if (!(leftViewPos.x > -0.1f && leftViewPos.x < 1.1f && leftViewPos.y > 0 && leftViewPos.y < 1 && leftViewPos.z > 0)){
            //
            //   //Check that terrain is outside of left eye view
            //   vert = transform.TransformPoint(vertices[terrainTiles[i].farRight[0]]);
            //   Vector3 rightViewPos = cam.WorldToViewportPoint(vert);
            //   if (!(rightViewPos.x > -0.1f && rightViewPos.x < 1.1f && rightViewPos.y > 0 && rightViewPos.y < 1 && rightViewPos.z > 0)){

                UpdateTerrain(i);
                EnvironmentPlacement(i);
              // }
            // }
          }
        }

        // PlaceEnemy(i);
      }

        //Update mesh
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    //TODO: replace with GetCurrentQuadrent() to get the quadrent of the verts. However up is different between the verts and the player
    /* DetermineQuadrent
      => Determine where vector belongs based on center point found between the the center and the vertex world values
      => Add the vert to the appropriate list
    */
    void DetermineQuadrent(Vector3 v, int i, float dist){

      float roundX = Mathf.Round(v.x * 1000f) / 1000f;
      float roundY = Mathf.Round(v.y * 1000f) / 1000f;

      //Quadrant 0
      if (roundX >= 0 && roundY > 0 && roundY > dist ) {
        terrainTiles[0].vertNum.Add(i);

      //Quadrant 1
      } else if (roundX > 0 && roundY > 0 && roundY <= dist) {
        terrainTiles[1].vertNum.Add(i);

      //Quadrant 2
      } else if (roundX > 0 && roundY <= 0 && roundY > (-1)*dist) {
        terrainTiles[2].vertNum.Add(i);

      //Quadrant 3
      } else if (roundX > 0 && roundY < 0 && roundY <= (-1)*dist) {
        terrainTiles[3].vertNum.Add(i);

      //Quadrant 4
      } else if (roundX <= 0 && roundY < 0 && roundY < (-1)*dist) {
        terrainTiles[4].vertNum.Add(i);

      //Quadrant 5
      } else if (roundX < 0 && roundY < 0 && roundY >= (-1)*dist) {
        terrainTiles[5].vertNum.Add(i);

      //Quadrant 6
      } else if (roundX < 0 && roundY >= 0 && roundY < dist) {
        terrainTiles[6].vertNum.Add(i);

      //Quadrant 7
      } else if(roundX < 0 && roundY > 0 && roundY >= dist) {
        terrainTiles[7].vertNum.Add(i);

      } else {
        Debug.Log("No Quadrent " + i + " " + roundY + " " + dist);
      }
    }


    /* DetermineVerticeGroups
      => Order the vertices within each quadrent into 4 groups (FarRight, MidRight, MidLeft, FarLeft)
      => Takes the angle of the vert based on its x and y and tests it
        => Starting at FarRight, it tests if the vertex has an equal angle to whats currently present in the list or if it has a greater/lesser angle
        => Equal gets added to the list
        => Lesser gets added to the list after being sorted via sortVerts()
        => Greater gets pushed to the next list to be tested
        => If List is empty, it gets added to the list
    */
    void DetermineVerticeGroups(){

      for (int i=0; i<terrainTiles.Length; i++){
        foreach (int j in terrainTiles[i].vertNum) {

          float angle = Mathf.Atan(vertices[j].x / vertices[j].y);
          angle = Mathf.Round(angle * 1000f) / 1000f;

          //FARRIGHT
          if (terrainTiles[i].farRight.Count > 0) {
            float compAngle = Mathf.Atan(vertices[terrainTiles[i].farRight[0]].x / vertices[terrainTiles[i].farRight[0]].y);
            compAngle = Mathf.Round(compAngle * 1000f) / 1000f;

            if (compAngle == angle){
              terrainTiles[i].farRight.Add(j);

            } else if (compAngle > angle){
              SortVerts(i, 0, j);

            } else {

              //MIDRIGHT
              if (terrainTiles[i].midRight.Count > 0) {
                compAngle = Mathf.Atan(vertices[terrainTiles[i].midRight[0]].x / vertices[terrainTiles[i].midRight[0]].y);
                compAngle = Mathf.Round(compAngle * 1000f) / 1000f;

                if (compAngle == angle){
                  terrainTiles[i].midRight.Add(j);

                } else if (compAngle > angle){
                  SortVerts(i, 1, j);

                } else {

                  //MIDLEFT
                  if (terrainTiles[i].midLeft.Count > 0) {
                    compAngle = Mathf.Atan(vertices[terrainTiles[i].midLeft[0]].x / vertices[terrainTiles[i].midLeft[0]].y);
                    compAngle = Mathf.Round(compAngle * 1000f) / 1000f;

                    if (compAngle == angle){
                    terrainTiles[i].midLeft.Add(j);

                    } else if (compAngle > angle){
                      SortVerts(i, 2, j);

                    } else {

                      //FARLEFT
                      if (terrainTiles[i].farLeft.Count > 0) {
                        compAngle = Mathf.Atan(vertices[terrainTiles[i].farLeft[0]].x / vertices[terrainTiles[i].farLeft[0]].y);
                        compAngle = Mathf.Round(compAngle * 1000f) / 1000f;

                        if (compAngle == angle){
                          terrainTiles[i].farLeft.Add(j);

                        } else {
                          Debug.Log("Vert not placed");
                        }

                      } else {
                        terrainTiles[i].farLeft.Add(j);

                      }
                    }

                  } else {
                    terrainTiles[i].midLeft.Add(j);
                  }
                }

              } else {
                terrainTiles[i].midRight.Add(j);
              }
            }

          } else {
            terrainTiles[i].farRight.Add(j);
          }
        }
      }
    }


    /* SortVerts
      => Given a vertex with a lesser angle then what was present in the list (vertGroup)
        => Replace the old vertex with the new one and push the old one to be added in the next list
        => Uses Recurtion to get through all the lists
    */
    void SortVerts(int quadNum, int vertGroup, int newValue){
      /*
        0 = FR
        1 = MR
        2 = ML
        3 = FL
      */

      int temp;

      switch(vertGroup){
        case 0: //Swap FarRight and MidRight
          temp = terrainTiles[quadNum].farRight[0];
          terrainTiles[quadNum].farRight.Remove(temp);
          terrainTiles[quadNum].farRight.Add(newValue);

          if (terrainTiles[quadNum].midRight.Count > 0){
            SortVerts(quadNum, 1, temp);
          } else {
            terrainTiles[quadNum].midRight.Add(temp);
          }
          break;

        case 1: //Swap MidRight and MidLeft
          temp = terrainTiles[quadNum].midRight[0];
          terrainTiles[quadNum].midRight.Remove(temp);
          terrainTiles[quadNum].midRight.Add(newValue);

          if (terrainTiles[quadNum].midLeft.Count > 0){
            SortVerts(quadNum, 2, temp);
          } else {
            terrainTiles[quadNum].midLeft.Add(temp);
          }
          break;

        case 2: //Swap MidLeft and FarLeft
          temp = terrainTiles[quadNum].midLeft[0];
          terrainTiles[quadNum].midLeft.Remove(temp);
          terrainTiles[quadNum].midLeft.Add(newValue);
          terrainTiles[quadNum].farLeft.Add(temp);
          break;

      }
    }


    /* TerrainConstraints
      => Check if player is on or near updating tile
      => Check if enemy is present on updating tile
    */
    private bool TerrainConstraints(int tileNum){
      bool playerConstraint = false;

      //Test if player is on or next to terrain
      if (player != null){

        //Test for outliers (0, 7)
        if (curPlayerQuad == 0){
          if (tileNum != 7 && tileNum != 1 && tileNum != 0){
            playerConstraint = true;
          }
        } else if (curPlayerQuad == 7){
          if (tileNum != 0 && tileNum != 6 && tileNum != 7){
            playerConstraint = true;
          }
        } else if (tileNum != (curPlayerQuad - 1) && tileNum != (curPlayerQuad + 1) && tileNum != curPlayerQuad){
          playerConstraint = true;
        }
      }

      //Test if any enemies are on the quad
      bool enemyConstraint = true;

      // for (int i=0; i<enemySpawn.GetNumEnemies(); i++){
      //
      //   //TODO: test this to make sure its working properly (enemies get bumped around a lot, so at times it might not be right)
      //
      //   int enemyQuad = GetCurrentQuadrent(enemySpawn.GetEnemyPosition(i));
      //
      //   if (tileNum == enemyQuad){
      //     enemyConstraint = false;
      //   }
      // }

      if (playerConstraint && enemyConstraint){
        return true;
      } else {
        return false;
      }
    }


    /* GetCurrentQuadrent
      => Calculates the angle from the pos to a know refference point (terrainTile 0's FL)
      => Compare angle to known angles of other terrains
      => Return which terrain contains the angle
    */
    public int GetCurrentQuadrent(Vector3 pos){

      //NOTE: the vertices.z represents up, were as for the player and enemies .y represents up
      //TODO: Possible update to player and enemy so .z represents up (might happen when models are put in)

      float angle;
      float sign;
      Vector3 refPoint = new Vector3(vertices[terrainTiles[0].farLeft[0]].x, vertices[terrainTiles[0].farLeft[0]].y, 0);
      Vector3 posVector = new Vector3(pos.z, pos.x, 0);

      //Establish angle from refPoint(terrainTile 0) to player
      angle = Vector3.Angle(refPoint, posVector);
      sign = Mathf.Sign(Vector3.Dot(new Vector3(0, 0, 1), Vector3.Cross(refPoint, posVector)));

      angle = angle * sign;
      angle = (angle + 180) % 360;

      //NOTE: This puts it in the right place since the player and enimies are skewed till we get moddels made
      angle -= 90f;
      if (angle < 0){
        angle = 360 + angle;
      }
      //Angles for tiles 0: 180, 1: 135, 2: 90, 3: 45, 4: 0, 5: 315, 6: 270, 7: 225

      if (angle >= 0f && angle < 45f){
        return 4;

      } else if (angle >= 45f && angle < 90f){
        return 3;

      } else if (angle >= 90f && angle < 135f){
        return 2;

      } else if (angle >= 135f && angle < 180f){
        return 1;

      } else if (angle >= 180f && angle < 225f){
        return 0;

      } else if (angle >= 225f && angle < 270f){
        return 7;

      } else if (angle >= 270f && angle < 315f){
        return 6;

      } else if (angle >= 315f){
        return 5;
      }

      return -1;
    }


    /* GetCenterPointsFromQuad
      => Get the center points between the verts of farLeft, midLeft, midRight, farRight
      => Based on specified quadrent
    */
    public Vector3[] GetCenterPointsFromQuad(int quadNum){

      Vector3[] centerPoints = new Vector3[3];
      Vector3 point1;
      Vector3 point2;
      Vector3 farPoint;
      Vector3 closePoint;

      //TODO will need to place constraints based on path taken, where the player is, Height of quadrent, ect. Just to check that it is a viable option

      //FarLeft - MidLeft
      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].farLeft[3]].x, vertices[terrainTiles[quadNum].farLeft[3]].y, vertices[terrainTiles[quadNum].farLeft[3]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midLeft[3]].x, vertices[terrainTiles[quadNum].midLeft[3]].y, vertices[terrainTiles[quadNum].midLeft[3]].z));
      farPoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].farLeft[0]].x, vertices[terrainTiles[quadNum].farLeft[0]].y, vertices[terrainTiles[quadNum].farLeft[0]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midLeft[0]].x, vertices[terrainTiles[quadNum].midLeft[0]].y, vertices[terrainTiles[quadNum].midLeft[0]].z));
      closePoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      centerPoints[0] = Vector3.Lerp(new Vector3(closePoint.x, closePoint.y, closePoint.z), new Vector3(farPoint.x, farPoint.y, farPoint.z), 0.5f);

      //MidLeft - MidRight
      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midLeft[3]].x, vertices[terrainTiles[quadNum].midLeft[3]].y, vertices[terrainTiles[quadNum].midLeft[3]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midRight[3]].x, vertices[terrainTiles[quadNum].midRight[3]].y, vertices[terrainTiles[quadNum].midRight[3]].z));
      farPoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midLeft[0]].x, vertices[terrainTiles[quadNum].midLeft[0]].y, vertices[terrainTiles[quadNum].midLeft[0]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midRight[0]].x, vertices[terrainTiles[quadNum].midRight[0]].y, vertices[terrainTiles[quadNum].midRight[0]].z));
      closePoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      centerPoints[1] = Vector3.Lerp(new Vector3(closePoint.x, closePoint.y, closePoint.z), new Vector3(farPoint.x, farPoint.y, farPoint.z), 0.5f);

      //MidRight - FarRight
      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midRight[3]].x, vertices[terrainTiles[quadNum].midRight[3]].y, vertices[terrainTiles[quadNum].midRight[3]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].farRight[3]].x, vertices[terrainTiles[quadNum].farRight[3]].y, vertices[terrainTiles[quadNum].farRight[3]].z));
      farPoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      point1 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].midRight[0]].x, vertices[terrainTiles[quadNum].midRight[0]].y, vertices[terrainTiles[quadNum].midRight[0]].z));
      point2 = transform.TransformPoint(new Vector3(vertices[terrainTiles[quadNum].farRight[0]].x, vertices[terrainTiles[quadNum].farRight[0]].y, vertices[terrainTiles[quadNum].farRight[0]].z));
      closePoint = Vector3.Lerp(new Vector3(point1.x, point1.y, point1.z), new Vector3(point2.x, point2.y, point2.z), 0.5f);

      centerPoints[2] = Vector3.Lerp(new Vector3(closePoint.x, closePoint.y, closePoint.z), new Vector3(farPoint.x, farPoint.y, farPoint.z), 0.5f);

      return centerPoints;
    }
}
