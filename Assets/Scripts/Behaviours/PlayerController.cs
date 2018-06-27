using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using Commands;
  using TileMap;

  [RequireComponent(typeof(ClickToInteract))]
  [RequireComponent(typeof(MapEntity))]
  [RequireComponent(typeof(Inventory))]
  public class PlayerController : MonoBehaviour {
    /**
    * The map properties of the player.
    */
    public MapEntity mapEntity;

    public Inventory inventory;

    public ClickToInteract clickToInteract;

    void OnEnable() {
      StateManager.Instance.playerController = this;
    }

    void OnDisable() {
      StateManager.Instance.playerController = null;
    }

    // Use this for initialization
    void Awake() {
    }

    void Start() {
    }

    // Update is called once per frame
    void Update () {

    }
  }
}
