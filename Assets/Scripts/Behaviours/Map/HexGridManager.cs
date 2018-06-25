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

  /**
   * \brief
   * Gets the metrics of the current map.
   */
  public HexMetrics hexMetrics => CurrentGrid?.Map.metrics;

  void Awake () {
    Debug.Assert(null != StateManager.Instance, "state manager exists");
    StateManager.Instance.hexGridManager = this;
    LoadMap(initialMap);
  }

  /**
   * \brief
   * Destroys the currently loaded map.
   *
   * This is a no-op if there is no loaded map.
   */
  public void DestroyMap() {
    if(null != CurrentGrid)
      Destroy(CurrentGrid.gameObject);
  }

  /**
   * \brief
   * Calls `DestroyImmediate` on the current HexGrid's GameObject, if the
   * grid exists. The #CurrentGrid property is nulled out.
   */
  public void DestroyMapImmediate() {
    if(null == CurrentGrid)
      return;
    DestroyImmediate(CurrentGrid.gameObject);
    CurrentGrid = null;
  }

  /**
   * \brief
   * Loads a new map.
   * First destroys the current map, if any.
   */
  public HexGrid LoadMap(HexMap map) {
    DestroyMap();

    var obj = GameObject.Instantiate(gridPrefab, transform);
    CurrentGrid = obj.GetComponent<HexGrid>();
    Debug.Assert(CurrentGrid != null, "gridPrefab GameObject contains a HexGrid component.");
    CurrentGrid.Map = map;
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
          CurrentGrid.Map.initialPlayerY,
          CurrentGrid.hexMetrics));

      // if there's a script to run in the new map, then run it.
      if(null != CurrentGrid.Map.mapLoad)
        StateManager.Instance.eventManager.BeginScript(CurrentGrid, CurrentGrid.Map.mapLoad);
    }
  }
}
