using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Commands;
  using TileMap;
  using Variables;

  [RequireComponent(typeof(HasInventory))]
  public class PlayerController : MonoBehaviour {
    public Character character;

    void Awake() {
    }

    void OnEnable() {
    }

    void OnDisable() {
    }
  }
}
