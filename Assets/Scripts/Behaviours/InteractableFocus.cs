using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Focuses the nearest interactable that is close enough to the
   * player.
   */
  [RequireComponent(typeof(IsoGrid))]
  public class InteractableFocus : MonoBehaviour {
    [SerializeField] [HideInInspector]
    PlayerController player;

    [SerializeField] [HideInInspector]
    IsoGrid grid;

    /**
     * \brief
     * The currently focused interactable.
     */
    public Interactable focus;

    void Awake() {
      grid = GetComponent<IsoGrid>();
      Debug.Assert(
        null != grid,
        "InteractableFocus must be attached to an IsoGrid.");
    }

    void OnPlayerSpawned(IMap _grid) {
      Debug.Assert(
        _grid == grid,
        "Grid that fired in the same as grid we registered on.");
      player = grid.Player;
    }

    void OnEnable() {
      grid.PlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable() {
      grid.PlayerSpawned -= OnPlayerSpawned;
    }

    void Update() {
      // find all interactables close enough to the player
      // sort them by distance.
      // set the closest as the focused one.
      if(player == null)
        return;

      if(focus != null) focus.focused = false;
      focus = FindClosestInteractableInRange();
      if(focus != null) focus.focused = true;
    }

    /**
     * \brief
     * Finds the closest Interactable to the player.
     *
     * \returns
     * `null` if there are no interactables in the map, or if none are
     * in range of the player.
     */
    Interactable FindClosestInteractableInRange() {
      Interactable closest = null;
      float lastDistance = Single.PositiveInfinity;
      var p = player.transform.position;

      float currentDistance = 0;
      foreach(var interactable in grid.Interactables) {
        if(interactable.IsInRangeOf(p, out currentDistance)
           && currentDistance < lastDistance) {
          closest = interactable;
          lastDistance = currentDistance;
        }
      }

      return closest;
    }
  }
}
