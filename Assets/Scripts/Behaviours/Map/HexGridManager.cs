using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * \brief
 * A controller for the HexMap model.

 * The HexGrid represents a hexagonal grid map.
 */
public class HexGridManager : MonoBehaviour {
  /**
   * \brief
   * The current managed map.
   */
  public HexGrid CurrentGrid {
    get;
    private set;
  }

  /**
   * \brief
   * The initial map to load.
   * THIS IS FOR TESTING!
   */
  public HexMap initialMap;

  /**
   * \brief
   * The prefab used to create the player.
   */
  public GameObject playerPrefab;

  /**
   * \brief
   * The prefab used to create maps.
   */
  public GameObject gridPrefab;

  void Awake () {
    Debug.Assert(null != StateManager.Instance, "state manager exists");
    StateManager.Instance.hexGridManager = this;
    LoadMap(initialMap);
  }

  private void DestroyMap() {
    Destroy(CurrentGrid.gameObject);
  }

  /**
   * \brief
   * Loads a new map.
   * Destroys the current map, if any.
   */
  public HexGrid LoadMap(HexMap map) {
    if(CurrentGrid != null)
      DestroyMap();

    var obj = GameObject.Instantiate(gridPrefab, transform);
    CurrentGrid = obj.GetComponent<HexGrid>();
    Debug.Assert(CurrentGrid != null, "gridPrefab GameObject contains a HexGrid component.");
    CurrentGrid.Setup(map);
    return CurrentGrid;
  }

  public PlayerController SpawnPlayer() {
    var obj = Instantiate(playerPrefab, CurrentGrid.transform);
    return obj.GetComponent<PlayerController>();
  }

  void Start() {
    if(CurrentGrid != null) {
      var player = SpawnPlayer();
      var me = player.GetComponent<MapEntity>();

      me.Warp(
        HexCoordinates.FromOffsetCoordinates(
          CurrentGrid.Map.initialPlayerX,
          CurrentGrid.Map.initialPlayerY));

      // if there's a script to run in the new map, then run it.
      if(null != CurrentGrid.Map.mapLoad)
        StateManager.Instance.eventManager.BeginScript(CurrentGrid, CurrentGrid.Map.mapLoad);
    }
  }
}
