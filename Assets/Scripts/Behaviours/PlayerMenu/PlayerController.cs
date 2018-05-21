using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[RequireComponent(typeof(ClickToInteract))]
[RequireComponent(typeof(MapEntity))]
public class PlayerController : MonoBehaviour {
  /**
   * The map properties of the player.
   */
  public MapEntity mapEntity;

  public ClickToInteract clickToInteract;

  void OnEnable() {
    StateManager.Instance.playerController = this;
  }

  void OnDisable() {
    StateManager.Instance.playerController = null;
  }

	// Use this for initialization
	void Awake() {
    mapEntity = GetComponent<MapEntity>();
    clickToInteract = GetComponent<ClickToInteract>();
	}

  void Start() {
  }

	// Update is called once per frame
	void Update () {
		
	}
}
