using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TerrainMain : MonoBehaviour
{

    /* UpdateTerrain
      => Remove any previously added vertices and triangles used for cliffs
      => Randomly choose terrain focus (flat, uphill, downhill)
      => Based on choice, randomly adjust verticeGroups to obtain terrain focus, but staying in line with connecting tiles
    */
    void UpdateTerrain(int tileNum){

      //Check if added verts and triangles exist
      if (terrainTiles[tileNum].addedVerts[0] != -1){

        //Check if cliff have been created afterwards and need to be moved within the array
        if (terrainTiles[tileNum].addedVerts[1] + 1 < vertices.Length){

          UpdateVertices(tileNum);
          UpdateTriangles(tileNum);

        } else {
          //need to reset vertices and triangle array (removing the verts and triangles and shrinking the array)
          Debug.Log("Single cliff change");
        }

        //Reset addedVerts
        for (int i=0; i<terrainTiles[tileNum].addedVerts.Length; i++){
          terrainTiles[tileNum].addedVerts[i] = -1;
        }

        //Reset addedTriangles
        for (int i=0; i<terrainTiles[tileNum].addedTriangles.Length; i++){
          terrainTiles[tileNum].addedTriangles[i] = -1;
        }
      }

      DetermineTerrainFocus(tileNum);

      terrainTiles[tileNum].isExplored = false;
    }


    /*UpdateVertices
      => Move vertices with a higher index down
      => Adjust triangles array removing 2 slots
      => Update each tiles addedVerts if affected
      => Update affected verts within the triangles array
    */
    private void UpdateVertices(int tileNum){

      //Move vertices down
      for (int i = terrainTiles[tileNum].addedVerts[0]; i<vertices.Length - 2; i++){
        vertices[i] = vertices[i + 2];
      }

      //Ajust vertices array to new size
      Vector3[] newVertsArray = new Vector3[vertices.Length - 2];

      for (int i=0; i<vertices.Length - 2; i++){
        newVertsArray[i] = vertices[i];
      }

      vertices = newVertsArray;

      //Update tiles indexs in addedVerts
      int counter = 0;

      for (int i=0; i<terrainTiles.Length; i++){
        if (terrainTiles[i].addedVerts[0] != -1){

          if (terrainTiles[i].addedVerts[0] > terrainTiles[tileNum].addedVerts[0]){

            counter++; // keep track of how many tiles needed to be changed

            for (int j=0; j<terrainTiles[i].addedVerts.Length; j++){
              terrainTiles[i].addedVerts[j] -= 2;
            }
          }
        }
      }

      //Based on the number of tiles changed, check verts with in the triangles array 
      for (int i=triangles.Length - counter*15; i<triangles.Length; i++){
        if (triangles[i] > terrainTiles[tileNum].addedVerts[1]){
          triangles[i] -= 2;
        }
      }
    }


    /*UpdateTriangles
      => Move triangles with a higher index down
      => Adjust triangles array removing 15 slots
      => Update each tiles addedTriangles if affected
    */
    private void UpdateTriangles(int tileNum){

      //Move triangles down
      for (int i = terrainTiles[tileNum].addedTriangles[0]; i<triangles.Length - 15; i++){
        triangles[i] = triangles[i + 15];
      }

      //Ajust triangles array
      int[] newTrianglesArray = new int[triangles.Length - 15];

      for (int i=0; i<triangles.Length - 15; i++){
        newTrianglesArray[i] = triangles[i];
      }

      triangles = newTrianglesArray;

      //Update tiles addedTriangles
      for (int i=0; i<terrainTiles.Length; i++){
        if (terrainTiles[i].addedVerts[0] != -1){

          if (terrainTiles[i].addedTriangles[0] > terrainTiles[tileNum].addedTriangles[0]){

            for (int j=0; j<15; j++){
              terrainTiles[i].addedTriangles[j] -= 15;
            }
          }
        }
      }
    }


    /* DetermineTerrainFocus
      => Randomly determine which terrain should be focused
    */
    private void DetermineTerrainFocus(int tileNum){

      int rand = Random.Range(1, 100);

      //Return flat focus
      if (rand <= 50){
        UpdateAsCWCliffTerrain(tileNum);
        //UpdateAsFlatTerrain(tileNum);

      //Return uphill focus
      } else if (rand > 50 && rand <= 70){
          UpdateAsCWCliffTerrain(tileNum);
        // UpdateAsUphillTerrain(tileNum);

      //Return downhill focus
      } else if (rand > 70 && rand <= 90){
          UpdateAsCWCliffTerrain(tileNum);
        // UpdateAsDownhillTerrain(tileNum);

      } else if (rand > 90 && rand <= 100){
        UpdateAsCWCliffTerrain(tileNum);
      // //Return cliff focus
      // } else if (rand > 90 && rand <= 95){
      //
      // //Return pit swing focus
      // } else {
      //
      }
    }


    /* UpdateAsFlatTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: -0.01 -> 0.004
        => diff < 0: -0.004 -> 0.01
    */
    private void UpdateAsFlatTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farLeft[0]].z;
      }


      //FarRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 6f, -2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 3f, -2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farRight[j]].z = height;
        }

        //Set new FarRight to nextTileHeight
        nextTileHeight = height;


      //FarLeft
        diff = prevTileHeight - nextTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 6f, -2f, prevTileHeight);

        } else {
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 3f, -2f, prevTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farLeft[j]].z = height;
        }

        //Set new FarLeft to prevTileHeight
        prevTileHeight = height;


      //MidRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 6f, -2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 9); //1-8
          height = UpdateGroupHeight(randNum, 3f, -2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midRight[j]].z = height;
        }

        //Set new midRight to nextTileHeight
        nextTileHeight = height;


      //MidLeft (close the gap)
        diff = prevTileHeight - nextTileHeight;

        //Determine if ML or MR is lower
        if (diff > 0){
          //FL is heigher
          maxLow = prevTileHeight - 0.02f;
          maxHeight = nextTileHeight + 0.02f;

        } else {
          //MR is heigher
          maxLow = nextTileHeight - 0.02f;
          maxHeight = prevTileHeight + 0.02f;
        }

        //Determine how many multiples of 0.002f there are
        height = maxHeight - maxLow;
        height /= 0.002f;

        //Randomize how far from maxLow
        height = Random.Range(1, height+1);
        height *= 0.002f;

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midLeft[j]].z = (maxLow + height);
        }
    }


    /* UpdateAsUphillTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: 0.002 -> 0.012
        => diff < 0: 0.01 -> 0.02
    */
    private void UpdateAsUphillTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farLeft[0]].z;
      }


      //FarRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 7f, -2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 11f, -2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farRight[j]].z = height;
        }

        //Set new FarRight to nextTileHeight
        nextTileHeight = height;


      //FarLeft
        diff = prevTileHeight - nextTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          UpdateGroupHeight(randNum, 7f, -2f, prevTileHeight);

        } else {
          randNum = Random.Range(1, 7); //1-6
          UpdateGroupHeight(randNum, 11f, -2f, prevTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farLeft[j]].z = height;
        }

        //Set new FarLeft to prevTileHeight
        prevTileHeight = height;


      //MidRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 7f, -2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 11f, -2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midRight[j]].z = height;
        }

        //Set new midRight to nextTileHeight
        nextTileHeight = height;


      //MidLeft (close the gap)
        diff = prevTileHeight - nextTileHeight;

        //Determine if ML or MR is lower
        if (diff > 0){
          //FL is heigher
          maxLow = prevTileHeight - 0.02f;
          maxHeight = nextTileHeight + 0.02f;

        } else {
          //MR is heigher
          maxLow = nextTileHeight - 0.02f;
          maxHeight = prevTileHeight + 0.02f;
        }

        //Determine how many multiples of 0.002f there are
        height = maxHeight - maxLow;
        height /= 0.002f;

        //Randomize how far from maxLow
        height = Random.Range(1, height+1);
        height *= 0.002f;

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midLeft[j]].z = (maxLow + height);
        }
    }


    /* UpdateAsDownhillTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: -0.02 -> -0.01
        => diff < 0: -0.012 -> -0.002
    */
    private void UpdateAsDownhillTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farLeft[0]].z;
      }


      //FarRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 11f, 2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 7f, 2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farRight[j]].z = height;
        }

        //Set new FarRight to nextTileHeight
        nextTileHeight = height;


      //FarLeft
        diff = prevTileHeight - nextTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 11f, 2f, nextTileHeight);

        } else {
          //Determine height increase from -0.012 -> -0.002
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 7f, 2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farLeft[j]].z = height;
        }

        //Set new FarLeft to prevTileHeight
        prevTileHeight = height;


      //MidRight
        diff = nextTileHeight - prevTileHeight;

        //Determine if focusing upwards or downward
        if (diff > 0){
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 11f, 2f, nextTileHeight);

        } else {
          randNum = Random.Range(1, 7); //1-6
          height = UpdateGroupHeight(randNum, 7f, 2f, nextTileHeight);
        }

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midRight[j]].z = height;
        }

        //Set new midRight to nextTileHeight
        nextTileHeight = height;


      //MidLeft (close the gap)
        diff = prevTileHeight - nextTileHeight;

        //Determine if ML or MR is lower
        if (diff > 0){
          //FL is heigher
          maxLow = prevTileHeight - 0.02f;
          maxHeight = nextTileHeight + 0.02f;

        } else {
          //MR is heigher
          maxLow = nextTileHeight - 0.02f;
          maxHeight = prevTileHeight + 0.02f;
        }

        //Determine how many multiples of 0.002f there are
        height = maxHeight - maxLow;
        height /= 0.002f;

        //Randomize how far from maxLow
        height = Random.Range(1, height+1);
        height *= 0.002f;

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midLeft[j]].z = (maxLow + height);
        }
    }


    /* UpdateAsCliffTerrain

    */
    private void UpdateAsCWCliffTerrain(int tileNum){

      //TODO: Determine CW or CCW

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;
      float heightSign;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farLeft[0]].z;
      }

      //TODO: Might randomize height diffrence and if cliff goes upward or down
      // => diff can help determine the 2f value, whether pos or neg
      diff = nextTileHeight - prevTileHeight;

      //Testing
      diff = 1;
      randNum = 10;

      if (diff > 0){
        //upward
        heightSign = -2f;
      } else {
        //downward
        heightSign = 2f;
      }


      //FarRight
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, heightSign, nextTileHeight);

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farRight[j]].z = height;
        }

        nextTileHeight = height;


      //midRight
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, heightSign, nextTileHeight);

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midRight[j]].z = height;
        }

        nextTileHeight = height;


      //midLeft
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, heightSign, nextTileHeight);

        //Add new height to vert group
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].midLeft[j]].z = height;
        }

        nextTileHeight = height;


      //farLeft
        for (int j=0; j<4; j++){
          vertices[terrainTiles[tileNum].farLeft[j]].z = prevTileHeight;
        }


      //New Verts
      diff = nextTileHeight - prevTileHeight;
      diff += 0.01f;

      for (int i=0; i<terrainTiles[tileNum].addedVerts.Length; i++){
        terrainTiles[tileNum].addedVerts[i] = vertices.Length + i;
      }

      Vector3 newVert1 = new Vector3(vertices[terrainTiles[tileNum].farLeft[0]].x, vertices[terrainTiles[tileNum].farLeft[0]].y, vertices[terrainTiles[tileNum].farLeft[0]].z + diff);
      Vector3 newVert2 = new Vector3(vertices[terrainTiles[tileNum].farLeft[1]].x, vertices[terrainTiles[tileNum].farLeft[1]].y, vertices[terrainTiles[tileNum].farLeft[1]].z + diff);

      Vector3[] newVertices = new Vector3[vertices.Length + 2];
      vertices.CopyTo(newVertices, 0);
      newVertices[newVertices.Length - 2] = newVert1;
      newVertices[newVertices.Length - 1] = newVert2;

      vertices = newVertices;

      //Testing
      Debug.Log("Verts added to vertices");
      Debug.Log("Vertices 1: " + vertices[vertices.Length - 2].x + " " + vertices[vertices.Length - 2].y + " " + vertices[vertices.Length - 2].z);
      Debug.Log("Vertices 2: " + vertices[vertices.Length - 1].x + " " + vertices[vertices.Length - 1].y + " " + vertices[vertices.Length - 1].z);
      Debug.Log("Verts added to addedVerts");
      Debug.Log("Index 1: " + terrainTiles[tileNum].addedVerts[0] + " Vertices 1: " + vertices[terrainTiles[tileNum].addedVerts[0]].x + " " + vertices[terrainTiles[tileNum].addedVerts[0]].y + " " + vertices[terrainTiles[tileNum].addedVerts[0]].z);
      Debug.Log("Index 2: " + terrainTiles[tileNum].addedVerts[1] + " Vertices 2: " + vertices[terrainTiles[tileNum].addedVerts[1]].x + " " + vertices[terrainTiles[tileNum].addedVerts[1]].y + " " + vertices[terrainTiles[tileNum].addedVerts[1]].z);

      //New Triangles
      for (int i=0; i<terrainTiles[tileNum].addedTriangles.Length; i++){
        terrainTiles[tileNum].addedTriangles[i] = triangles.Length + i;
      }

      int[] newTriangles = new int[triangles.Length + 15];
      triangles.CopyTo(newTriangles, 0);

      //Side
      newTriangles[newTriangles.Length - 15] = vertices.Length - 2;
      newTriangles[newTriangles.Length - 14] = terrainTiles[tileNum].midLeft[0];
      newTriangles[newTriangles.Length - 13] = terrainTiles[tileNum].farLeft[0];

      //Cliff
      newTriangles[newTriangles.Length - 12] = vertices.Length - 2;
      newTriangles[newTriangles.Length - 11] = terrainTiles[tileNum].farLeft[0];
      newTriangles[newTriangles.Length - 10] = terrainTiles[tileNum].farLeft[1];

      newTriangles[newTriangles.Length - 9] = terrainTiles[tileNum].farLeft[1];
      newTriangles[newTriangles.Length - 8] = vertices.Length - 1;
      newTriangles[newTriangles.Length - 7] = vertices.Length - 2;

      //Cliff edge
      newTriangles[newTriangles.Length - 6] = vertices.Length - 1;
      newTriangles[newTriangles.Length - 5] = terrainTiles[tileNum].midLeft[1];
      newTriangles[newTriangles.Length - 4] = terrainTiles[tileNum].midLeft[0];

      newTriangles[newTriangles.Length - 3] = terrainTiles[tileNum].midLeft[0];
      newTriangles[newTriangles.Length - 2] = vertices.Length - 2;
      newTriangles[newTriangles.Length - 1] = vertices.Length - 1;

      triangles = newTriangles;
    }


    /* UpdateAsCCWCliffTerrain

    */
    private void UpdateAsCCWCliffTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farLeft[0]].z;
      }

      //TODO: determine up vs down cliff based on diff and which direction
    }


    /* UpdateGroupHeight
      => height = ((random height addOn) - X) * (Y) / 1000
      => Uses this equation to determine height adjustment of a vert group
    */
    private float UpdateGroupHeight(float randNum, float X, float Y, float adjacentTileHeight){

      float height;

      height = ((randNum - X) * (Y)) / 1000f;

      //Test Constraints for max and min height
      if (adjacentTileHeight + height > 0.1f){
        height = 0.1f;
      } else if (adjacentTileHeight + height < 0.0f){
        height = 0.0f;
      } else {
        height += adjacentTileHeight;
      }

      return height;
    }


    /*UpdateToTerrainCWUpwardCliff
      => Update terrain tile to a clockwise upward cliff
    */
    private void UpdateToTerrainCWUpwardCliff(int i, float prevTileHeight, float nextTileHeight){

      float num;

      //Move outer verts

      //Move FR to MR position
      for(int j=0; j<4; j++){
          vertices[terrainTiles[i].farRight[j]].x = vertices[terrainTiles[i].midRight[j]].x;
          vertices[terrainTiles[i].farRight[j]].y = vertices[terrainTiles[i].midRight[j]].y;
      }


      //FarLeft
      num = Random.Range(1, 7);
      num = ((num - 9f) * (-2f) / 1000f);

      if (prevTileHeight + num > 0.1f){
        num = 0.1f;
      } else {
        num += prevTileHeight;
      }

      //Add new height to vert group
      for (int j=0; j<4; j++){
        vertices[terrainTiles[i].farLeft[j]].z = num;
      }

      //Set new prevTileHeight
      prevTileHeight = num;


      //MidLeft
      num = Random.Range(1, 7);
      num = ((num - 9f) * (-2f) / 1000f);

      if (prevTileHeight + num > 0.1f){
        num = 0.1f;
      } else {
        num += prevTileHeight;
      }

      //Add new height to vert group
      for (int j=0; j<4; j++){
        vertices[terrainTiles[i].midLeft[j]].z = num;
      }

      //Set new prevTileHeight
      prevTileHeight = num;


      //MidRight
      num = Random.Range(1, 7);
      num = ((num - 9f) * (-2f) / 1000f);

      if (prevTileHeight + num > 0.1f){
        num = 0.1f;
      } else {
        num += prevTileHeight;
      }

      //Add new height to vert group
      for (int j=0; j<4; j++){
        vertices[terrainTiles[i].midRight[j]].z = num;
      }

      //Set new prevTileHeight
      prevTileHeight = num;


      //FarRight
      num = Random.Range(1, 7);
      num = ((num - 11f) * (-2f) / 1000f);

      for (int j=0; j<4; j++){
        vertices[terrainTiles[i].farRight[j]].z = nextTileHeight + num;
      }
    }


    /* UpdateToTerrainCWDownwardCLiff
      => Update terrain tile to a clockwise downward cliff
    */
    private void UpdateToTerrainCWDownwardCLiff(int i, float prevTileHeight, float nextTileHeight){

    }


    /* UpdateToTerrainCCWUpwardCliff
      => Update terrain tile to a counter clockwise upward cliff
    */
    private void UpdateToTerrainCCWUpwardCliff(int i, float prevTileHeight, float nextTileHeight){

    }


    /* UpdateToTerrainCCWDownwardCliff
      => Update terrain tile to a counter clockwise downward cliff
    */
    private void UpdateToTerrainCCWDownwardCliff(int i, float prevTileHeight, float nextTileHeight){

    }
}
