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
    }
  }
}
