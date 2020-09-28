using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TerrainMain : MonoBehaviour
{

    /* UpdateTerrain
      => Randomly choose terrain focus (flat, uphill, downhill)
      => Based on choice, randomly adjust verticeGroups to obtain terrain focus, but staying in line with connecting tiles
    */
    void UpdateTerrain(int i){
      //Change Terrain
      //TODO: Determine when curtain terrain should be placed (waterfall, swings, cliffs, ...)
      //Note: vertical cliff and pit swing will be affected by the distance traveled of the player

      float prevTileHeight;
      float nextTileHeight;
      float diff;

      //Test for outliers for previous tile height (0)
      if (i == 0){
        prevTileHeight = vertices[terrainTiles[7].farRight[0]].z;

      } else {
        prevTileHeight = vertices[terrainTiles[i-1].farRight[0]].z;
      }

      //Test for outliers for the next tile height (7)
      if (i == 7){
        nextTileHeight = vertices[terrainTiles[0].farLeft[0]].z;

      } else {
        nextTileHeight = vertices[terrainTiles[i+1].farLeft[0]].z;
      }

      //Determine focus of terrain
      int terrainRand = DetermineTerrainFocus(i);
      float num;
      float maxLow, maxHeight;

      switch(terrainRand){

        //Flat Terrain
        case 0:

          terrainTiles[i].type = TerrainType.Flat;

          //FarRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.01 -> 0.004
            num = Random.Range(1, 9); //1-8
            num = ((num - 6f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from -0.004 -> 0.01
            num = Random.Range(1, 9); //1-8
            num = ((num - 3f) * (-2f)) / 1000f;
          }

          //Test Constraints for max and min height
          if (nextTileHeight + num > 0.1f){
            num = 0.1f;
          } else if (nextTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farRight[j]].z = num;
          }

          //Set new FarRight to nextTileHeight
          nextTileHeight = num;


          //FarLeft

          diff = prevTileHeight - nextTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.01 -> 0.004
            num = Random.Range(1, 9); //1-8
            num = ((num - 6f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from -0.004 -> 0.01
            num = Random.Range(1, 9); //1-8
            num = ((num - 3f) * (-2f)) / 1000f;
          }

          //Test Constraints for max and min height
          if (prevTileHeight + num > 0.1f){
            num = 0.1f;
          } else if (prevTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += prevTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farLeft[j]].z = num;
          }

          //Set new FarLeft to prevTileHeight
          prevTileHeight = num;


          //MidRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.01 -> 0.004
            num = Random.Range(1, 9); //1-8
            num = ((num - 6f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from -0.004 -> 0.01
            num = Random.Range(1, 9); //1-8
            num = ((num - 3f) * (-2f)) / 1000f;
          }

          //Test Constraints for max and min height
          if (nextTileHeight + num > 0.1f){
            num = 0.1f;
          } else if (nextTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midRight[j]].z = num;
          }

          //Set new midRight to nextTileHeight
          nextTileHeight = num;


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
          num = maxHeight - maxLow;
          num /= 0.002f;

          //Randomize how far from maxLow
          num = Random.Range(1, num+1);
          num *= 0.002f;

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midLeft[j]].z = (maxLow + num);
          }

          break;


        //Uphill Terrain
        case 1:
          terrainTiles[i].type = TerrainType.Uphill;

          //FarRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from 0.002 -> 0.012
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from 0.01 -> 0.02
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (-2f)) / 1000f;
          }

          //Test Constraints for max height
          if (nextTileHeight + num > 0.1f){
            num = 0.1f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farRight[j]].z = num;
          }

          //Set new FarRight to nextTileHeight
          nextTileHeight = num;


          //FarLeft

          diff = prevTileHeight - nextTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from 0.01 -> 0.02
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from 0.002 -> 0.01
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (-2f)) / 1000f;
          }

          //Test Constraints for max height
          if (prevTileHeight + num > 0.1f){
            num = 0.1f;
          } else {
            num += prevTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farLeft[j]].z = num;
          }

          //Set new FarLeft to prevTileHeight
          prevTileHeight = num;


          //MidRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.01 -> 0.004
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (-2f)) / 1000f;

          } else {
            //Determine height increase from -0.004 -> 0.01
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (-2f)) / 1000f;
          }

          //Test Constraints for max height
          if (nextTileHeight + num > 0.1f){
            num = 0.1f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midRight[j]].z = num;
          }

          //Set new midRight to nextTileHeight
          nextTileHeight = num;


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
          num = maxHeight - maxLow;
          num /= 0.002f;

          //Randomize how far from maxLow
          num = Random.Range(1, num+1);
          num *= 0.002f;

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midLeft[j]].z = (maxLow + num);
          }

          break;


        //Downhill Terrain
        case 2:
          terrainTiles[i].type = TerrainType.Downhill;

          //FarRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.02 -> -0.01
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (2f)) / 1000f;

          } else {
            //Determine height increase from -0.012 -> -0.002
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (2f)) / 1000f;
          }

          //Test Constraints for min height
          if (nextTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farRight[j]].z = num;
          }

          //Set new FarRight to nextTileHeight
          nextTileHeight = num;


          //FarLeft

          diff = prevTileHeight - nextTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.02 -> -0.01
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (2f)) / 1000f;

          } else {
            //Determine height increase from -0.012 -> -0.002
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (2f)) / 1000f;
          }

          //Test Constraints for min height
          if (nextTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].farLeft[j]].z = num;
          }

          //Set new FarLeft to prevTileHeight
          prevTileHeight = num;


          //MidRight

          diff = nextTileHeight - prevTileHeight;

          //Determine if focusing upwards or downward
          if (diff > 0){

            //Determine height increase from -0.02 -> -0.01
            num = Random.Range(1, 7); //1-6
            num = ((num - 11f) * (2f)) / 1000f;

          } else {
            //Determine height increase from -0.012 -> -0.002
            num = Random.Range(1, 7); //1-6
            num = ((num - 7f) * (2f)) / 1000f;
          }

          //Test Constraints for min height
          if (nextTileHeight + num < 0.0f){
            num = 0.0f;
          } else {
            num += nextTileHeight;
          }

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midRight[j]].z = num;
          }

          //Set new midRight to nextTileHeight
          nextTileHeight = num;


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
          num = maxHeight - maxLow;
          num /= 0.002f;

          //Randomize how far from maxLow
          num = Random.Range(1, num+1);
          num *= 0.002f;

          //Add new height to vert group
          for (int j=0; j<4; j++){
            vertices[terrainTiles[i].midLeft[j]].z = (maxLow + num);
          }

          break;


        //Cliff terrain
        //TODO: create cw vs ccw cliffs
        //NOTE: possible idea is to have hill then drop off or a drop off then a hill
        case 3:
          terrainTiles[i].type = TerrainType.Cliff;
          Debug.Log("Cliff Terrain");

          //CW
          //Cliff later

          UpdateToTerrainCWUpwardCliff(i, prevTileHeight, nextTileHeight);
          //UpdateToTerrainCWDownwardCLiff(i, prevTileHeight, nextTileHeight);
          //UpdateToTerrainCCWUpwardCliff(i, prevTileHeight, nextTileHeight);
          //UpdateToTerrainCCWDownwardCliff(i, prevTileHeight, nextTileHeight);




          break;


        //Pit Swing terrain
        case 4:
          Debug.Log("Pit Swing");
          break;
      }

      terrainTiles[i].isExplored = false;
    }


    /* DetermineTerrainFocus
      => Randomly determine which terrain should be focused
    */
    int DetermineTerrainFocus(int i){

      int rand = Random.Range(1, 100);

      //Return flat focus
      if (rand <= 50){
        return 0;

      //Return uphill focus
      } else if (rand > 50 && rand <= 70){
        return 1;

      //Return downhill focus
      } else if (rand > 70 && rand <= 90){
        return 2;

      //Return cliff focus
      } else if (rand > 90 && rand <= 95){
        return 0; //change to 3

      //Return pit swing focus
      } else {
        return 0; //change to 4
      }
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
