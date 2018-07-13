using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Sets the object's `Z` coordinate according to the `Y` coordinate
   * of their floor.
   */
  public class Floor : MonoBehaviour {
    /**
     * \brief
     * The distance from the pivot, as an offset on the Y axis.
     */
    public float distanceFromPivot;

    public float gizmoXOffset;

    public const float HALF_WIDTH = 1;

    void LateUpdate() {
      var p = transform.position;
      p.z = p.y + distanceFromPivot;
      transform.position = p;
    }

    void OnDrawGizmos() {
      // make a copy of the target's position, so we can modify it.
      Gizmos.color = Color.yellow;
      var v = transform.position;
      v.x += gizmoXOffset;
      v.y += distanceFromPivot;
      var start = v;
      start.x -= HALF_WIDTH;
      var end = v;
      end.x += HALF_WIDTH;
      Gizmos.DrawLine(start, end);
    }
  }
}
