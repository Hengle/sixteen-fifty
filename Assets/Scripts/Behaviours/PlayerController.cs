using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[RequireComponent(typeof(MapEntity))]
public class PlayerController : MonoBehaviour {
  // set by Construct
  public HexGrid grid;

  /**
   * The map properties of the player.
   */
  public MapEntity mapEntity;

  public static PlayerController Construct(GameObject prefab, HexGrid grid) {
    var self = prefab.GetComponent<PlayerController>();
    self.grid = grid;
    var instance = Instantiate(prefab).GetComponent<PlayerController>();
    self.grid = null;

    instance.transform.parent = grid.transform;

    instance.mapEntity.Warp(grid[HexCoordinates.Zero]);

    return instance;
  }

  void OnBeginMove(MapEntity me) {
    Debug.Assert(me == mapEntity);
    DisableClickToMove();
  }

  void OnEndMove(MapEntity me) {
    Debug.Assert(me == mapEntity);
    EnableClickToMove();
  }

  void OnEnable() {
    EnableClickToMove();
    StateManager.Instance.playerController = this;
    mapEntity.BeginMove += OnBeginMove;
    mapEntity.EndMove += OnEndMove;
  }

  void OnDisable() {
    DisableClickToMove();
    StateManager.Instance.playerController = null;
    mapEntity.BeginMove -= OnBeginMove;
    mapEntity.EndMove -= OnEndMove;
  }

  void EnableClickToMove() {
    grid.CellDown += OnCellDown;
  }

  void DisableClickToMove() {
    grid.CellDown -= OnCellDown;
  }

  /**
   * Event handler for cell clicks.
   */
  void OnCellDown(HexCell cell) {
    if(cell == mapEntity.CurrentCell)
      return;
    
    var path = grid.FindPath(mapEntity.CurrentCell.coordinates, cell.coordinates);
    if(null == path) {
      Debug.LogError("No path can be found.");
      return;
    }

    mapEntity.MoveFollowingPath(path);
  }

	// Use this for initialization
	void Awake() {
    mapEntity = this.GetComponentNotNull<MapEntity>();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
