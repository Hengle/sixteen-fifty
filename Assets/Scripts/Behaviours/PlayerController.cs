using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using Commands;
  using TileMap;
  using Variables;

  [RequireComponent(typeof(ClickToInteract))]
  [RequireComponent(typeof(MapEntity))]
  [RequireComponent(typeof(Inventory))]
  public class PlayerController : MonoBehaviour {
    /**
     * \brief
     * The map properties of the player.
     * Set in the inspector.
     */
    public MapEntity mapEntity;

    /**
     * \brief
     * The player's inventory.
     */
    public Inventory inventory;

    /**
     * \brief
     * Indicates where the player should be positioned when it loads.
     */
    public HexCoordinatesVariable destination;

    [SerializeField] [HideInInspector]
    HexGridManager manager;

    void Awake() {
      manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != manager,
        "PlayerController is instantiated in a map context.");
    }

    void OnEnable() {
      manager.GridLoaded += OnGridLoaded;
    }

    void OnDisable() {
      manager.GridLoaded -= OnGridLoaded;
    }

    void OnGridLoaded(HexGrid grid) {
      if(null != destination)
        mapEntity.Warp(destination.Value);
    }
  }
}
