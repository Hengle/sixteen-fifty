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
  public class AxisMovement : MonoBehaviour, INotifyDirectionChange {
    public float moveSpeed = 1;

    [SerializeField] [HideInInspector]
    HexGridManager manager;
    
    [SerializeField] [HideInInspector]
    Rigidbody2D body;

    [SerializeField] [HideInInspector]
    HexDirection lastDirection;

    public event Action<HexDirection> DirectionChanged;
    
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

      // only bother updating direction if someone is listening for it,
      // and if we're actually moving
      if(DirectionChanged != null && v.sqrMagnitude < 0.01)
        return;

      var theta = Mathf.Atan2(v.y, v.x);
      if(theta < 0) theta += Mathf.PI * 2;
      var d = TileMap.HexMetrics.DirectionFromAngle(theta);
      if(d != lastDirection) {
        lastDirection = d;
        DirectionChanged(d);
      }
    }
  }
}
