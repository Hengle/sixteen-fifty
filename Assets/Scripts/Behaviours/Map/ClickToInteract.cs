using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using UI;
  using TileMap;
  
  [RequireComponent(typeof(MapEntity))]
  public class ClickToInteract : MonoBehaviour {
    MapEntity mapEntity;
    bool interactionsEnabled;
    InteractionMenu InteractionMenu => eventManager.interactionMenu;
    EventManager eventManager;

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

      eventManager = StateManager.Instance.eventManager;
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
    * Collects all interactions available on the given cell and shows
    * the interaction menu with those choices.
    */
    void PresentInteractionsMenu(HexCell cell) {
      var interactions =
        cell.EntitiesHere
        // get the interactable component of each mapentity on the cell
        // and throw out the non-interactable ones
        .Select(me => me.GetComponent<Interactable>())
        .Where(ictb => null != ictb)
        // fish out the interactions from each interactable
        .SelectMany(ictb => ictb.npcData.interactions)
        .ToArray();

      if(interactions.Length == 0)
        return;

      InteractionMenu.Show(interactions, OnMenuInteracted);
    }

    void OnMenuInteracted(Interaction interaction) {
      // unregister ourselves from the menu.
      InteractionMenu.Interacted -= OnMenuInteracted;
      if(interaction == null)
        return;
      eventManager.BeginScript(mapEntity.Grid.Manager, interaction.script.root);
    }

    /**
    * Event handler for cell clicks.
    */
    void OnCellDown(HexCell cell) {
      var d = cell.coordinates.DistanceTo(mapEntity.CurrentCell.coordinates);

      // did we click on ourselves?
      if(0 == d) {
        PresentInteractionsMenu(cell);
        return;
      }

      // did we click on an adjacent nonempty tile?
      if(1 == d && cell.IsNonEmpty) {
        PresentInteractionsMenu(cell);
        return;
      }

      // otherwise, we clicked somewhere that will require us to move

      if(cell.IsNonEmpty) {
        // however, we can't move somewhere where there's something!
        Debug.Log("refusing to move somewhere where there's something! d = " + d.ToString());
        return;
      }

      // otherwise, the clicked cell is empty, so we pathfind to it and move.
      var path = mapEntity.Grid.FindPath(mapEntity.CurrentCell.coordinates, cell.coordinates);
      if(null == path) {
        Debug.LogError("No path can be found.");
        return;
      }
      mapEntity.MoveFollowingPath(path);
    }
  }
}
