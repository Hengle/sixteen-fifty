using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapEntity))]
public class ClickToInteract : MonoBehaviour {
  MapEntity mapEntity;
  bool interactionsEnabled;

  void Awake() {
    interactionsEnabled = false;
    mapEntity = GetComponent<MapEntity>();
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
    mapEntity.BeginMove += OnBeginMove;
    mapEntity.EndMove += OnEndMove;
  }

  void OnDisable() {
    DisableInteractions();
    mapEntity.BeginMove -= OnBeginMove;
    mapEntity.EndMove -= OnEndMove;
  }

  /**
   * \brief
   * Enables interactivity for the entity.
   * This is a no-op if interactivity is already enabled.
   */
  void EnableInteractions() {
    if(interactionsEnabled)
      return;
    mapEntity.Grid.CellDown += OnCellDown;
  }

  /**
   * \brief
   * Disables interactivity for the entity.
   * This is a no-op if interactivity is already disabled.
   */
  void DisableInteractions() {
    if(!interactionsEnabled)
      return;
    mapEntity.Grid.CellDown -= OnCellDown;
  }

  /**
   * \brief
   * Collects Interaction objects from adjacent tiles and launches a
   * ControlInteractionMenu script.
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
      mapEntity.Grid,
      new ControlInteractionMenu(this.transform.position, interactions));
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
    
    var path = mapEntity.Grid.FindPath(mapEntity.CurrentCell.coordinates, cell.coordinates);
    if(null == path) {
      Debug.LogError("No path can be found.");
      return;
    }

    mapEntity.MoveFollowingPath(path);
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
