using System;
using System.Linq;
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
    DisableInteractions();
  }

  void OnEndMove(MapEntity me) {
    Debug.Assert(me == mapEntity);
    EnableInteractions();
  }

  void OnEnable() {
    EnableInteractions();
    StateManager.Instance.playerController = this;
    mapEntity.BeginMove += OnBeginMove;
    mapEntity.EndMove += OnEndMove;
  }

  void OnDisable() {
    DisableInteractions();
    StateManager.Instance.playerController = null;
    mapEntity.BeginMove -= OnBeginMove;
    mapEntity.EndMove -= OnEndMove;
  }

  void EnableInteractions() {
    grid.CellDown += OnCellDown;
  }

  void DisableInteractions() {
    grid.CellDown -= OnCellDown;
  }

  /**
   * This is actually an event script.
   */
  void PresentInteractionsMenu() {
    var interactions =
      mapEntity.CurrentCell.Neighbours
      // get every map entity on every neighbouring cell, and ourselves
      .SelectMany(cell => cell.EntitiesHere)
      .Concat(new [] { mapEntity })
      // get the interactable component of each mapentity and throw
      // out the non-interactable ones
      .Select(me => me.GetComponent<Interactable>())
      .Where(ictb => null != ictb)
      // fish out the interactions from each interactable
      .SelectMany(ictb => ictb.npcData.interactions)
      .ToList();

    if(interactions.Count == 0)
      return;

    StateManager.Instance.eventManager.BeginScript(
      grid,
      new ControlInteractionMenu(this, interactions));
  }



  /**
   * Event handler for cell clicks.
   */
  void OnCellDown(HexCell cell) {
    if(cell == mapEntity.CurrentCell) {
      PresentInteractionsMenu();
      return;
    }

    if(cell.EntitiesHere.Count > 0) {
      Debug.Log("Can't move into a cell with something else on it!");
      return;
    }
    
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
