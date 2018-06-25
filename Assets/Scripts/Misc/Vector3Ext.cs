using UnityEngine;

/**
 * \brief
 * Extension methods for the Vector3 class.
 */
public static class Vector3Ext {
  /**
   * \brief
   * Constructs a Vector2 from this Vector3 by dropping the Z component.
   */
  public static Vector2 Downgrade(this Vector3 self) {
    return new Vector2(self.x, self.y);
  }
}
