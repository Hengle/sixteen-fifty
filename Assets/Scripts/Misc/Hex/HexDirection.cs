using System;

/**
 * A direction that can be moved in from the point of view of a
 * hexagon.
 */
[Serializable]
public enum HexDirection {
  North, NorthEast, SouthEast, South, SouthWest, NorthWest
}

/**
 * \brief
 * Extension methods for HexDirection.
 */
public static class HexDirectionExt {
  /**
   * \brief
   * Inverts a direction.
   */
  public static HexDirection Opposite(this HexDirection d) =>
    (int)d < 3 ? (d + 3) : (d - 3);

  /**
   * \brief
   * Collapses HexDirection::NorthWest and HexDirection::SouthWest.
   */
  public static bool IsWest(this HexDirection d) =>
    d == HexDirection.NorthWest
    || d == HexDirection.SouthWest;

  /**
   * \brief
   * Collapses HexDirection::NorthEast and HexDirection::SouthEast.
   */
  public static bool IsEast(this HexDirection d) =>
    d == HexDirection.NorthEast
    || d == HexDirection.SouthEast;

  /**
   * \brief
   * Collapses all northern directions.
   */
  public static bool IsNorthern(this HexDirection d) =>
    d == HexDirection.North
    || d == HexDirection.NorthWest
    || d == HexDirection.NorthEast;

  public static bool IsSouthern(this HexDirection d) =>
    d == HexDirection.South
    || d == HexDirection.SouthWest
    || d == HexDirection.SouthEast;
}
