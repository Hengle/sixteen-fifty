using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class HexMetrics {
  /**
   * The half-width of the ellipse into which we inscribe our hexagon.
   * Mathematically, this is paramater _a_ of the ellipse.
   */
  public const float OUTER_WIDTH = 1f;

  /**
   * The half-height of the hexagon.
   */
  public const float INNER_HEIGHT = 0.5f;

  /**
   * The half-height of the ellipse into which we inscribe our hexagon.
   * Mathematically, this is parameter _b_ of the ellipse.
   */
  public static readonly float OUTER_HEIGHT = INNER_HEIGHT / Mathf.Sin(Mathf.PI/3);

  /**
   * The width of a hexagon.
   */
  public const float FULL_WIDTH = OUTER_WIDTH * 2;

  /**
   * The height of a hexagon.
   */
  public static readonly float FULL_HEIGHT = INNER_HEIGHT * 2;

  /**
   * The half-width of the hexagon.
   * This coincides with the half-width of the circumscribed ellipse.
   */
  public static readonly float INNER_WIDTH = OUTER_WIDTH;

  /**
   * The half-width of the top edge of the hexagon.
   */
  public static readonly float TOP_WIDTH = (float) Ecos(Mathf.PI / 3);

  /**
   * The x-distance between two adjacent (on the x-axis) hexes.
   */
  public static readonly float CENTER_DISTANCE_X = OUTER_WIDTH + TOP_WIDTH;

  /**
   * The determinant of the matrix H that converts hex-space to pixel-space.
   */
  public static readonly float D =
    INNER_WIDTH * (1 + (float) Math.Cos(-Math.PI/3)) * 2 * INNER_HEIGHT;

  /** The corners of the hexagons. */
  public static Vector2[] corners;

  /** Compute an elliptic sine. */
  public static float Esin(float theta) {
    return Mathf.Sin(theta) * OUTER_HEIGHT;
  }

  /** Compute an elliptic cos. */
  public static float Ecos(float theta) {
    return Mathf.Cos(theta) * OUTER_WIDTH;
  }

  /**
   * Computes a rectangle that encloses the hex-map with the given width and height.
   * Remark: the rectangle will have the correct *size* but might not be in the correct *place*.
   * (It should typically be off by one rectangle half-width and half-height.)
   */
  public static Vector2 Bounds(int width, int height) {
    // idea: pretend like the hexagons are arranged one next to the
    // other (not with nice offsetting)
    float w = width * FULL_WIDTH;
    // now because we have to squish the hexagons, we have to account
    // for the overlapping regions
    w -= (width - 1) * (OUTER_WIDTH - TOP_WIDTH);
    // account for the possibility that width is zero, which would
    // cause the product with (width - 1) to give a negative value of
    // w.
    if(w < 0)
      w = 0;

    // there's no interlocking in the y dimension, so we don't need to
    // subtract for overlapping regions.
    float h = height * FULL_HEIGHT;
    // however, if width > 1, then offsetting will occur, so we need
    // to bump up the height by one half-height.
    if(width > 1)
      h += INNER_HEIGHT;

    return new Vector2(w, h);
  }

  /**
   * Determines whether the given point is enclosed within a hexagon at the origin.
   */
  public static bool Contains(Vector2 p) {
    // We traverse all the corners of the basic hexagon and check that
    // the point we're verifying lies on the RHS of each segment.
    // (We can check this using a cross product.)
    var l = corners.Length;
    for(int i = 0; i < l; i++) {
      var c = corners[i];
      // construct the vector representing the segment (which takes us
      // from the current corner to the next corner.
      var s = corners[(i + 1) % l] - c;
      // construct the vector that takes us from the current corner to
      // the point to verify.
      var q = p - c;

      // Since we're crossing x-y vectors, the resulting vector will
      // have only a z-component, which we pull out.
      var z = Vector3.Cross(new Vector3(q.x, q.y, 0), new Vector3(s.x, s.y, 0)).z;

      // The sign of this z component tells us whether p is to the
      // right or to the left of the segment, due to the right-hand
      // rule.  Since our corners are in clockwise order, we need p to
      // be to the right every time.
      // So if it's to the left at any iteration, we can abort with false.
      if(z < 0)
        return false;
    }

    // otherwise, it's to the right every time, so we can return true.
    return true;
  }

  static HexMetrics() {
    corners = Enumerable.Range(0, 6)
      .Select(i => -i * Mathf.PI / 3)
      .Select(t => new Vector2(Ecos(t), Esin(t)))
      .ToArray();

    Debug.Log("Initialized hex metrics.");
  }
}
