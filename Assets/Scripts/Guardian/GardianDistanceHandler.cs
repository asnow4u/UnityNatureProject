using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardianDistanceHandler : GardianController
{
  
    private int distance;
    private int tileLoc;
    private int curTileLoc;

    [SerializeField]
    private GameObject terrain;

    private TerrainMain terrainScript;

    void Awake() {
        terrainScript = terrain.GetComponent<TerrainMain>();
    }

    void Start() {
        distance = 0;
        tileLoc = terrainScript.GetCurrentQuadrent(transform.position);
    }

    void Update() {

      curTileLoc = terrainScript.GetCurrentQuadrent(transform.position);

      //Check for high edge cases
      if (tileLoc == 6 && curTileLoc == 0) {
        distance += 2;
        tileLoc = 0;
      }

      else if (tileLoc == 7 && curTileLoc == 1) {
        distance += 2;
        tileLoc = 1;
      }

      //Check for low edge cases
      else if (tileLoc == 0 && curTileLoc == 6) {
        distance += 2;
        tileLoc = 6;
      }

      else if (tileLoc == 1 && curTileLoc == 7) {
        distance += 2;
        tileLoc = 7;


      } else {

        if (curTileLoc == tileLoc + 2f || curTileLoc == tileLoc - 2f) {

          distance += 2;
          tileLoc = curTileLoc;
        }
      }
    }

}
