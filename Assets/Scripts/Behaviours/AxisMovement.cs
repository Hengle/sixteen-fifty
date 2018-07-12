using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Controls the velocity of a Rigidbody2D using the horizontal and
   * vertical axes.
   */
  [RequireComponent(typeof(Rigidbody2D))]
  public class AxisMovement : MonoBehaviour {
    public float moveSpeed = 1;

    [SerializeField] [HideInInspector]
    HexGridManager manager;
    
    [SerializeField] [HideInInspector]
    Rigidbody2D body;
    
    void Awake() {
      body = GetComponent<Rigidbody2D>();
      Debug.Assert(
        null != body,
        "AxisMovement is attached with Rigidbody2D.");

      manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != manager,
        "AxisMovement is under a HexGridManager.");
    }

    bool Active => !manager.eventManager.IsUI;

    void FixedUpdate() {
      if(!Active) {
        body.velocity = Vector2.zero;
        return;
      }

      var v = InputUtility.PrimaryAxis;
      body.velocity = v * moveSpeed;
    }
  }
}
