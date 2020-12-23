using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TerrainMain : MonoBehaviour
{

    /* DetermineTerrainFocus
      => Randomly determine which terrain should be focused
    */
    private void DetermineTerrainFocus(int tileNum){

      int rand = Random.Range(1, 100);

      //Return flat focus
      if (rand <= 50){
        UpdateAsFlatTerrain(tileNum);
        terrainTiles[tileNum].focus = terrainFocus.Flat;

      //Return uphill focus
      } else if (rand > 50 && rand <= 70){
        UpdateAsUphillTerrain(tileNum);
        terrainTiles[tileNum].focus = terrainFocus.Uphill;

      //Return downhill focus
      } else if (rand > 70 && rand <= 90){
        UpdateAsDownhillTerrain(tileNum);
        terrainTiles[tileNum].focus = terrainFocus.Downhill;

      //Return cliff focus
      } else if (rand > 90 && rand <= 95){
        UpdateAsCWCliffTerrain(tileNum);

      } else {
        UpdateAsCWCliffTerrain(tileNum);
        // UpdateAsCCWCliffTerrain(tileNum);
      }

      //Test
      Debug.Log("TileNum: " + tileNum + " " + terrainTiles[tileNum].focus);
    }


    /* UpdateAsFlatTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height ajustment by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: -0.01 -> 0.004
        => diff < 0: -0.004 -> 0.01
      => If previously a CwCliff, CcwCliff, or Island, set appropriate height adjustment
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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[3] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[0] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[2] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[1] = height;


      //Cliff
        if (terrainTiles[tileNum].focus == terrainFocus.CwCliff){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.CcwCliff){
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.Island){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }
    }


    /* UpdateAsUphillTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height adjustment by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: 0.002 -> 0.012
        => diff < 0: 0.01 -> 0.02
      => If previously a CwCliff, CcwCliff, or Island, set appropriate height adjustment
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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[3] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[0] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[2] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[1] = height;


      //Cliff
        if (terrainTiles[tileNum].focus == terrainFocus.CwCliff){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.CcwCliff){
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.Island){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }
    }


    /* UpdateAsDownhillTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Height update range:
        => diff > 0: -0.02 -> -0.01
        => diff < 0: -0.012 -> -0.002
      => If previously a CwCliff, CcwCliff, or Island, set appropriate height adjustment
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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[3] = height;

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

        ///Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[0] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[2] = height;

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

        //Update new height adjustment
        terrainTiles[tileNum].heightAdjustment[1] = height;


      //Cliff
        if (terrainTiles[tileNum].focus == terrainFocus.CwCliff){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.CcwCliff){
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.Island){
          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
        }
    }


    /* UpdateAsCliffTerrain
      => Determine the height differnce between the next and previous tiles
      => Update height by vert groups (FarRight, FarLeft, MidRight, MidLeft)
      => Create new verts and triangles for the cliff
    */
    private void UpdateAsCWCliffTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farLeft[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farLeft[0]].z;
      }

      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farRight[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farRight[0]].z;
      }


      //FarRight
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, prevTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[3] = height;

        //Set height to top of CCWCliff if present
        if (terrainTiles[tileNum].focus == terrainFocus.CcwCliff){
          //NOTE: currently keeps the same height above FR.z
          height = vertices[terrainTiles[tileNum].addedVerts[2]].z + (height - vertices[terrainTiles[tileNum].farRight[0]].z);
          terrainTiles[tileNum].heightAdjustment[5] = height;
        }

        prevTileHeight = height;


      //midRight
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, prevTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[2] = height;

        prevTileHeight = height;


      //midLeft
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, prevTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[1] = height;

        prevTileHeight = height;


      //farLeft
        //NOTE: Might randomize this height, dont have to tho
        terrainTiles[tileNum].heightAdjustment[0] = nextTileHeight;


        diff = prevTileHeight - nextTileHeight;
        diff += 0.01f; //TODO: randomized value to adjust height

        terrainTiles[tileNum].heightAdjustment[4] = vertices[terrainTiles[tileNum].farLeft[0]].z + diff;

        //Check focus
        if (terrainTiles[tileNum].focus == terrainFocus.Flat || terrainTiles[tileNum].focus == terrainFocus.Uphill || terrainTiles[tileNum].focus == terrainFocus.Downhill || terrainTiles[tileNum].focus == terrainFocus.CcwCliff){

          //Create New Verts
          terrainTiles[tileNum].addedVerts[0] = vertices.Length;
          terrainTiles[tileNum].addedVerts[1] = vertices.Length + 1;

          Vector3 newVert1 = new Vector3(vertices[terrainTiles[tileNum].farLeft[0]].x, vertices[terrainTiles[tileNum].farLeft[0]].y, vertices[terrainTiles[tileNum].farLeft[0]].z);
          Vector3 newVert2 = new Vector3(vertices[terrainTiles[tileNum].farLeft[3]].x, vertices[terrainTiles[tileNum].farLeft[3]].y, vertices[terrainTiles[tileNum].farLeft[3]].z);

          Vector3[] newVertices = new Vector3[vertices.Length + 2];
          vertices.CopyTo(newVertices, 0);
          newVertices[newVertices.Length - 2] = newVert1;
          newVertices[newVertices.Length - 1] = newVert2;

          vertices = newVertices;

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
          newTriangles[newTriangles.Length - 10] = terrainTiles[tileNum].farLeft[3];

          newTriangles[newTriangles.Length - 9] = terrainTiles[tileNum].farLeft[3];
          newTriangles[newTriangles.Length - 8] = vertices.Length - 1;
          newTriangles[newTriangles.Length - 7] = vertices.Length - 2;

          //Cliff edge
          newTriangles[newTriangles.Length - 6] = vertices.Length - 1;
          newTriangles[newTriangles.Length - 5] = terrainTiles[tileNum].midLeft[3];
          newTriangles[newTriangles.Length - 4] = terrainTiles[tileNum].midLeft[0];

          newTriangles[newTriangles.Length - 3] = terrainTiles[tileNum].midLeft[0];
          newTriangles[newTriangles.Length - 2] = vertices.Length - 2;
          newTriangles[newTriangles.Length - 1] = vertices.Length - 1;

          triangles = newTriangles;

          //Set Focus
          if (terrainTiles[tileNum].focus == terrainFocus.CcwCliff){
            terrainTiles[tileNum].focus = terrainFocus.Island;
            Debug.Log("Island formed");

          } else {
            terrainTiles[tileNum].focus = terrainFocus.CwCliff;
          }
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.Island){
          //NOTE: currently keeps the same height above FL.z
          terrainTiles[tileNum].heightAdjustment[4] = vertices[terrainTiles[tileNum].addedVerts[0]].z + (terrainTiles[tileNum].heightAdjustment[0] - vertices[terrainTiles[tileNum].farLeft[0]].z);

          terrainTiles[tileNum].heightAdjustment[5] = terrainTiles[tileNum].heightAdjustment[3];
          terrainTiles[tileNum].focus = terrainFocus.CwCliff;
        }
    }



    /* UpdateAsCCWCliffTerrain

    */
    private void UpdateAsCCWCliffTerrain(int tileNum){

      float prevTileHeight, nextTileHeight;
      float height, diff, randNum;
      float maxLow, maxHeight;

      //Test for outliers for previous tile height (0)
      if (tileNum == 0){
        prevTileHeight = vertices[terrainTiles[7].farLeft[0]].z;
      } else {
        prevTileHeight = vertices[terrainTiles[tileNum-1].farLeft[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (tileNum == 7){
        nextTileHeight = vertices[terrainTiles[0].farRight[0]].z;
      } else {
        nextTileHeight = vertices[terrainTiles[tileNum+1].farRight[0]].z;
      }


      //FarLeft
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, nextTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[0] = height;

        //Set height to top of CWCliff if present
        if (terrainTiles[tileNum].focus == terrainFocus.CwCliff){
          //NOTE: currently keeps the same height above FR.z
          height = vertices[terrainTiles[tileNum].addedVerts[0]].z + (height - vertices[terrainTiles[tileNum].farLeft[0]].z);
          terrainTiles[tileNum].heightAdjustment[4] = height;
        }

        nextTileHeight = height;


      //midLeft
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, nextTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[1] = height;

        nextTileHeight = height;


      //midRight
        randNum = Random.Range(1, 10); //1-6
        height = UpdateGroupHeight(randNum, 20f, -2f, nextTileHeight);

        //Update height adjustment
        terrainTiles[tileNum].heightAdjustment[2] = height;

        nextTileHeight = height;


      //farRight
        //NOTE: Might randomize this height, dont have to tho
        terrainTiles[tileNum].heightAdjustment[3] = nextTileHeight;


        //New Verts
        diff = nextTileHeight - prevTileHeight;
        diff += 0.01f; //TODO: randomized value to adjust height

        terrainTiles[tileNum].heightAdjustment[5] = vertices[terrainTiles[tileNum].farRight[0]].z + diff;

        //Check focus
        if (terrainTiles[tileNum].focus == terrainFocus.Flat || terrainTiles[tileNum].focus == terrainFocus.Uphill || terrainTiles[tileNum].focus == terrainFocus.Downhill || terrainTiles[tileNum].focus == terrainFocus.CwCliff){

          //Create New Verts
          terrainTiles[tileNum].addedVerts[2] = vertices.Length;
          terrainTiles[tileNum].addedVerts[3] = vertices.Length + 1;

          Vector3 newVert1 = new Vector3(vertices[terrainTiles[tileNum].farRight[0]].x, vertices[terrainTiles[tileNum].farRight[0]].y, vertices[terrainTiles[tileNum].farRight[0]].z);
          Vector3 newVert2 = new Vector3(vertices[terrainTiles[tileNum].farRight[3]].x, vertices[terrainTiles[tileNum].farRight[3]].y, vertices[terrainTiles[tileNum].farRight[3]].z);

          Vector3[] newVertices = new Vector3[vertices.Length + 2];
          vertices.CopyTo(newVertices, 0);
          newVertices[newVertices.Length - 2] = newVert1;
          newVertices[newVertices.Length - 1] = newVert2;

          vertices = newVertices;

          //New Triangles
          for (int i=0; i<terrainTiles[tileNum].addedTriangles.Length; i++){
            terrainTiles[tileNum].addedTriangles[i] = triangles.Length + i;
          }

          int[] newTriangles = new int[triangles.Length + 15];
          triangles.CopyTo(newTriangles, 0);

          //Side
          newTriangles[newTriangles.Length - 15] = vertices.Length - 2;
          newTriangles[newTriangles.Length - 14] = terrainTiles[tileNum].farRight[0];
          newTriangles[newTriangles.Length - 13] = terrainTiles[tileNum].midRight[0];

          //Cliff
          newTriangles[newTriangles.Length - 12] = vertices.Length - 2;
          newTriangles[newTriangles.Length - 11] = terrainTiles[tileNum].farRight[3];
          newTriangles[newTriangles.Length - 10] = terrainTiles[tileNum].farRight[0];

          newTriangles[newTriangles.Length - 9] = terrainTiles[tileNum].farRight[3];
          newTriangles[newTriangles.Length - 8] = vertices.Length - 2;
          newTriangles[newTriangles.Length - 7] = vertices.Length - 1;

          //Cliff edge
          newTriangles[newTriangles.Length - 6] = vertices.Length - 1;
          newTriangles[newTriangles.Length - 5] = terrainTiles[tileNum].midRight[0];
          newTriangles[newTriangles.Length - 4] = terrainTiles[tileNum].midRight[3];

          newTriangles[newTriangles.Length - 3] = terrainTiles[tileNum].midRight[0];
          newTriangles[newTriangles.Length - 2] = vertices.Length - 1;
          newTriangles[newTriangles.Length - 1] = vertices.Length - 2;

          triangles = newTriangles;

          //Set focus
          if (terrainTiles[tileNum].focus == terrainFocus.CwCliff){
            terrainTiles[tileNum].focus = terrainFocus.Island;
            Debug.Log("Island formed");

          } else {
            terrainTiles[tileNum].focus = terrainFocus.CcwCliff;
          }
        }

        else if (terrainTiles[tileNum].focus == terrainFocus.Island){
          //NOTE: currently keeps the same height above FR.z
          terrainTiles[tileNum].heightAdjustment[5] = vertices[terrainTiles[tileNum].addedVerts[0]].z + (terrainTiles[tileNum].heightAdjustment[3] - vertices[terrainTiles[tileNum].farRight[0]].z);

          terrainTiles[tileNum].heightAdjustment[4] = terrainTiles[tileNum].heightAdjustment[0];
          terrainTiles[tileNum].focus = terrainFocus.CwCliff;
        }
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


    /* UpdateVertHeight
      => VertGroup: 0 = FL, 1 = ML, 2 = MR, 3 = FR
    */
    private void UpdateVertHeight(int tileNum, int vertGroup){

      switch(vertGroup){

        //FarLeft
        case 0:

          //Upward Movement
          if (terrainTiles[tileNum].heightAdjustment[0] > vertices[terrainTiles[tileNum].farLeft[0]].z){

            //Test if new height is over height goal
            if (vertices[terrainTiles[tileNum].farLeft[0]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[0]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farLeft[i]].z = terrainTiles[tileNum].heightAdjustment[0];
              }

              terrainTiles[tileNum].heightAdjustment[0] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farLeft[i]].z += adjustmentSpeed;
              }
            }

          //Downward movement
          } else {

            //Test if new height is under height goal
            if (vertices[terrainTiles[tileNum].farLeft[0]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[0]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farLeft[i]].z = terrainTiles[tileNum].heightAdjustment[0];
              }

              terrainTiles[tileNum].heightAdjustment[0] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farLeft[i]].z -= adjustmentSpeed;
              }
            }
          }

          break;

        //MidLeft
        case 1:

          //Upward Movement
          if (terrainTiles[tileNum].heightAdjustment[1] > vertices[terrainTiles[tileNum].midLeft[0]].z){

            //Test if new height is over height goal
            if (vertices[terrainTiles[tileNum].midLeft[0]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[1]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midLeft[i]].z = terrainTiles[tileNum].heightAdjustment[1];
              }

              terrainTiles[tileNum].heightAdjustment[1] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midLeft[i]].z += adjustmentSpeed;
              }
            }

          //Downward movement
          } else {

            //Test if new height is under height goal
            if (vertices[terrainTiles[tileNum].midLeft[0]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[1]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midLeft[i]].z = terrainTiles[tileNum].heightAdjustment[1];
              }

              terrainTiles[tileNum].heightAdjustment[1] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midLeft[i]].z -= adjustmentSpeed;
              }
            }
          }

          break;

        //MidRight
        case 2:

          //Upward Movement
          if (terrainTiles[tileNum].heightAdjustment[2] > vertices[terrainTiles[tileNum].midRight[0]].z){

            //Test if new height is over height goal
            if (vertices[terrainTiles[tileNum].midRight[0]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[2]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midRight[i]].z = terrainTiles[tileNum].heightAdjustment[2];
              }

              terrainTiles[tileNum].heightAdjustment[2] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midRight[i]].z += adjustmentSpeed;
              }
            }

          //Downward movement
          } else {

            //Test if new height is under height goal
            if (vertices[terrainTiles[tileNum].midRight[0]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[2]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midRight[i]].z = terrainTiles[tileNum].heightAdjustment[2];
              }

              terrainTiles[tileNum].heightAdjustment[2] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].midRight[i]].z -= adjustmentSpeed;
              }
            }
          }

          break;

        //FarRight
        case 3:

          //Upward Movement
          if (terrainTiles[tileNum].heightAdjustment[3] > vertices[terrainTiles[tileNum].farRight[0]].z){

            //Test if new height is over height goal
            if (vertices[terrainTiles[tileNum].farRight[0]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[3]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farRight[i]].z = terrainTiles[tileNum].heightAdjustment[3];
              }

              terrainTiles[tileNum].heightAdjustment[3] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farRight[i]].z += adjustmentSpeed;
              }
            }

          //Downward movement
          } else {

            //Test if new height is under height goal
            if (vertices[terrainTiles[tileNum].farRight[0]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[3]){

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farRight[i]].z = terrainTiles[tileNum].heightAdjustment[3];
              }

              terrainTiles[tileNum].heightAdjustment[3] = 0f;

            } else {

              for (int i=0; i<4; i++){
                vertices[terrainTiles[tileNum].farRight[i]].z -= adjustmentSpeed;
              }
            }
          }

          break;

        //CWCliff
        case 4:

          if (terrainTiles[tileNum].addedVerts[0] != -1){

            if (terrainTiles[tileNum].addedVerts[0] >= vertices.Length){
              Debug.Log(" Length: " + vertices.Length + " addedVerts[0]: " + terrainTiles[tileNum].addedVerts[0]);
            }

            //Upward Movement
            if (terrainTiles[tileNum].heightAdjustment[4] > vertices[terrainTiles[tileNum].addedVerts[0]].z){

              //Test if new height is over height goal
              if (vertices[terrainTiles[tileNum].addedVerts[0]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[4]){

                vertices[terrainTiles[tileNum].addedVerts[0]].z = terrainTiles[tileNum].heightAdjustment[4];
                vertices[terrainTiles[tileNum].addedVerts[1]].z = terrainTiles[tileNum].heightAdjustment[4];

                terrainTiles[tileNum].heightAdjustment[4] = 0f;

                //Test
                Debug.Log("Tile " + tileNum + " Cliff heightAdjustment set to 0f");

              } else {

                vertices[terrainTiles[tileNum].addedVerts[0]].z += adjustmentSpeed;
                vertices[terrainTiles[tileNum].addedVerts[1]].z += adjustmentSpeed;
              }

            //Downward movement
            } else {

              //Test if new height is under height goal
              if (vertices[terrainTiles[tileNum].addedVerts[0]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[4]){

                vertices[terrainTiles[tileNum].addedVerts[0]].z = terrainTiles[tileNum].heightAdjustment[4];
                vertices[terrainTiles[tileNum].addedVerts[1]].z = terrainTiles[tileNum].heightAdjustment[4];

                terrainTiles[tileNum].heightAdjustment[4] = 0f;

                //Test
                Debug.Log("Tile " + tileNum + " Cliff heightAdjustment set to 0f");

              } else {

                vertices[terrainTiles[tileNum].addedVerts[0]].z -= adjustmentSpeed;
                vertices[terrainTiles[tileNum].addedVerts[1]].z -= adjustmentSpeed;
              }
            }
          }

          break;

        //CCWCliff
        case 5:

          if (terrainTiles[tileNum].addedVerts[0] != -1){

            //Upward Movement
            if (terrainTiles[tileNum].heightAdjustment[5] > vertices[terrainTiles[tileNum].addedVerts[2]].z){

              //Test if new height is over height goal
              if (vertices[terrainTiles[tileNum].addedVerts[2]].z + adjustmentSpeed >= terrainTiles[tileNum].heightAdjustment[5]){

                vertices[terrainTiles[tileNum].addedVerts[2]].z = terrainTiles[tileNum].heightAdjustment[5];
                vertices[terrainTiles[tileNum].addedVerts[3]].z = terrainTiles[tileNum].heightAdjustment[5];

                terrainTiles[tileNum].heightAdjustment[5] = 0f;

                //Test
                Debug.Log("Tile " + tileNum + " Cliff heightAdjustment set to 0f");

              } else {

                vertices[terrainTiles[tileNum].addedVerts[2]].z += adjustmentSpeed;
                vertices[terrainTiles[tileNum].addedVerts[3]].z += adjustmentSpeed;
              }

            //Downward movement
            } else {

              //Test if new height is under height goal
              if (vertices[terrainTiles[tileNum].addedVerts[2]].z - adjustmentSpeed <= terrainTiles[tileNum].heightAdjustment[5]){

                vertices[terrainTiles[tileNum].addedVerts[2]].z = terrainTiles[tileNum].heightAdjustment[5];
                vertices[terrainTiles[tileNum].addedVerts[3]].z = terrainTiles[tileNum].heightAdjustment[5];

                terrainTiles[tileNum].heightAdjustment[5] = 0f;

                //Test
                Debug.Log("Tile " + tileNum + " Cliff heightAdjustment set to 0f");

              } else {

                vertices[terrainTiles[tileNum].addedVerts[2]].z -= adjustmentSpeed;
                vertices[terrainTiles[tileNum].addedVerts[3]].z -= adjustmentSpeed;
              }
            }
          }

          break;
      }
    }



    /* ResetTerrainTile
      => Remove any previously added vertices and triangles used for cliffs
      => If the tile is not at the end of the vertices array / triangles array, switch positions with the tile that is at the end before removing verts and triangles
        => Update the effected tiles vertices and triangles to reflect the position change within the vertices / triangles array 
    */
    void ResetTerrainTile(int tileNum, bool isCwCliff){

      int index1;
      int index2;

      //Grab relevent vert index based on CW or CCW cliff
      if (isCwCliff){
        index1 = terrainTiles[tileNum].addedVerts[0];
        index2 = terrainTiles[tileNum].addedVerts[1];

      } else {
        index1 = terrainTiles[tileNum].addedVerts[2];
        index2 = terrainTiles[tileNum].addedVerts[3];
      }

      //Check if tileNums addedVerts are at the end of the array, if not adjust the array
      if (index2 + 1 < vertices.Length){

        //Load end values into vertices[tileNum.addedVerts]
        vertices[index1] = vertices[vertices.Length - 2];
        vertices[index2] = vertices[vertices.Length - 1];

        //Find the tile that has the addedVert index for the end of the vertices array
        int updateTile = -1;

        for (int i=0; i<terrainTiles.Length; i++){

          //Cant be the same as tileNum
          if (i != tileNum){

            for (int j=0; j<4; j++){

              //addedVert cant be -1
              if (terrainTiles[i].addedVerts[j] != -1){

                if (terrainTiles[i].addedVerts[j] == vertices.Length - 1){
                  updateTile = i;
                  break;
                }
              }
            }
          }
        }

        Debug.Log("TileNum: " + tileNum + " updateTile: " + updateTile);


        //Update updateTiles addedVerts that contain the index for the end of the vertices array
        for (int j=0; j<terrainTiles[updateTile].addedVerts.Length; j++){

          if (terrainTiles[updateTile].addedVerts[j] != -1){

            if (terrainTiles[updateTile].addedVerts[j] == vertices.Length - 2){
              terrainTiles[updateTile].addedVerts[j] = index1;
            }

            if (terrainTiles[updateTile].addedVerts[j] == vertices.Length - 1){
              terrainTiles[updateTile].addedVerts[j] = index2;
            }
          }
        }

        //Update updateTiles addedTriangles that contain the index for the end of the vertices array
        for (int j=0; j<terrainTiles[updateTile].addedTriangles.Length; j++){

          Debug.Log("addedTriangle: " + j);

          Debug.Log("triangle Index: " + triangles[terrainTiles[updateTile].addedTriangles[j]] + " verticesLength-1: " + (vertices.Length - 1) + "verticesLength-2: " + (vertices.Length - 2));
          if (triangles[terrainTiles[updateTile].addedTriangles[j]] == vertices.Length - 2){
            triangles[terrainTiles[updateTile].addedTriangles[j]] = index1;
          }

          if (triangles[terrainTiles[updateTile].addedTriangles[j]] == vertices.Length - 1){
            triangles[terrainTiles[updateTile].addedTriangles[j]] = index2;
          }
        }


        //Load updateTile triangle values into tileNum current triangle location and update addedTriangles accordingly
        for (int i=0; i<15; i++){
          triangles[terrainTiles[tileNum].addedTriangles[i]] = triangles[terrainTiles[updateTile].addedTriangles[i]];
          terrainTiles[updateTile].addedTriangles[i] = terrainTiles[tileNum].addedTriangles[i];
        }
      }

      //Reset addedTriangles for tileNum
      for (int i=0; i<terrainTiles[tileNum].addedTriangles.Length; i++){
        terrainTiles[tileNum].addedTriangles[i] = -1;
      }

      //Reset addedVerts for tileNum
      for (int i=0; i<terrainTiles[tileNum].addedVerts.Length; i++){
        terrainTiles[tileNum].addedVerts[i] = -1;
      }


      //Remove end of triangles array
      int[] newTrianglesArray = new int[triangles.Length - 15];
      for (int i=0; i<triangles.Length - 15; i++){
        newTrianglesArray[i] = triangles[i];
      }
      triangles = newTrianglesArray;


      //Remove end of vertices array
      Vector3[] newVertsArray = new Vector3[vertices.Length - 2];
      for (int i=0; i<vertices.Length - 2; i++){
        newVertsArray[i] = vertices[i];
      }
      vertices = newVertsArray;

      Debug.Log("Tile Cliffed Removed From: " + tileNum);
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
}
