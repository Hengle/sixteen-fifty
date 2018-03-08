using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A direction that can be moved in from the point of view of a
 * hexagon.
 */
public enum HexDirection {
  North, NorthEast, SouthEast, South, SouthWest, NorthWest
}

public static class HexDirectionExt {
  /**
   * Inverts a direction.
   */
  public static HexDirection Opposite(this HexDirection d)
  => (int)d < 3 ? (d + 3) : (d - 3);
}
