using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Serialization;

  [RequireComponent(typeof(HasInventory))]
  public class PlayerController : SerializableBehaviour {
    public Character character;

    [SerializeField] [HideInInspector]
    public IPositioner positioner;

    void Awake() {
      positioner = GetComponent(typeof(IPositioner))
        as IPositioner;
      Debug.Assert(
        null != positioner,
        "PlayerController is with an IPositioner.");
    }

    void OnEnable() {
    }

    void OnDisable() {
    }
  }
}
