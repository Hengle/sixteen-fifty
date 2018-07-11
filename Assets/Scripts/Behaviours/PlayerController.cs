using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty.Behaviours {
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

    public Character character;

    void Awake() {
    }

    void OnEnable() {
    }

    void OnDisable() {
    }
  }
}
