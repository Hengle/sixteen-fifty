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

  void OnEnable() {
    EnableClickToMove();
    StateManager.Instance.playerController = this;
  }

  void EnableClickToMove() {
    grid.CellDown += OnCellDown;
  }

  void OnDisable() {
    DisableClickToMove();
    StateManager.Instance.playerController = null;
  }

  void DisableClickToMove() {
    grid.CellDown -= OnCellDown;
  }

  /**
   * Event handler for cell clicks.
   */
  void OnCellDown(HexCell cell) {
    if(cell == mapEntity.CurrentCell) {
      StateManager.Instance.eventManager.BeginScript(
        new ExampleEventScript());
    }
    else {
      if(mapEntity.IsMoving) {
        Debug.LogError("tried to start moving, but we're already moving.");
        return;
      }
      var path = grid.FindPath(mapEntity.CurrentCell.coordinates, cell.coordinates);
      if(null == path) {
        Debug.LogError("No path can be found.");
        return;
      }

      StartCoroutine(
        Command<object>.Action(() => { DisableClickToMove(); })
        .Then(_ => mapEntity.MoveFollowingPath(path))
        .ThenAction(_ => { EnableClickToMove(); })
        .GetCoroutine());
    }
  }

	// Use this for initialization
	void Awake() {
    mapEntity = this.GetComponentNotNull<MapEntity>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
