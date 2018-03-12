using System;

/**
 * A direction that can be moved in from the point of view of a
 * hexagon.
 */
[Serializable]
public enum HexDirection {
  North, NorthEast, SouthEast, South, SouthWest, NorthWest
}

public static class HexDirectionExt {
  /**
   * Inverts a direction.
   */
  public static HexDirection Opposite(this HexDirection d)
  => (int)d < 3 ? (d + 3) : (d - 3);

  public static bool IsWest(this HexDirection d) {
    return d == HexDirection.NorthWest || d == HexDirection.SouthWest;
  }

  public static bool IsEast(this HexDirection d) {
    return d == HexDirection.NorthEast || d == HexDirection.SouthEast;
  }
}
