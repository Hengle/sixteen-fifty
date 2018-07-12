using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SixteenFifty.TileMap {
  using Variables;
  
  [CreateAssetMenu(menuName = "1650/Hex Metrics")]
  public class HexMetrics : ScriptableObject {
    private static HexDirection[] directions = new [] {
        HexDirection.NorthEast,
        HexDirection.North,
        HexDirection.NorthWest,
        HexDirection.SouthWest,
        HexDirection.South,
        HexDirection.SouthEast
      };

    /**
     * \brief
     * Decides which direction is the primary direction of the given
     * angle according to these metrics.
     *
     * \param angle
     * The angle in *radians* to reverse, in `[0, 2pi)`.
     */
    public HexDirection DirectionFromAngle(float angle) {
      var t = Mathf.PI / 3f;
      for(var i = 0; i < 6; i++) {
        var theta = i * t;
        if(theta <= angle && angle < theta + t)
          return directions[i];
      }

      throw new SixteenFiftyException(
        String.Format("Impossible hex angle {0}*pi.", angle));
    }

    /**
    * \brief
    * The PPU value used on *all* art assets.
    */
    public IntVariable PIXELS_PER_UNIT;

    /**
    * \brief
    * The half-width of the ellipse into which the hexagon is
    * inscribed.
    * This is parameter _a_ of the ellipse.
    */
    public float OUTER_HALF_WIDTH =>
      // because our hexagons are flat-topped, the outer half-width and
      // the inner half-width are the same.
      INNER_HALF_WIDTH;

    /**
    * \brief
    * The half-height of the ellipse into which the hexagon is
    * inscribed.
    * this is parameter _b_ of the ellipse.
    */
    public float OUTER_HALF_HEIGHT =>
      INNER_HALF_HEIGHT / Mathf.Sin(Mathf.PI / 3);

    /**
    * \brief
    * Half the width of a hexagon.
    */
    public float INNER_HALF_WIDTH;

    /**
    * \brief
    * Half the height of a hexagon.
    */
    public float INNER_HALF_HEIGHT;

    /**
    * \brief
    * The full height of a hexagon.
    */
    public float INNER_HEIGHT =>
      INNER_HALF_HEIGHT * 2;

    /**
    * \brief
    * The full width of a hexagon.
    */
    public float INNER_WIDTH =>
      INNER_HALF_WIDTH * 2;

    /**
    * \brief
    * The width of the top edge of the hexagon.
    */
    public float TOP_WIDTH =>
      Ecos(Mathf.PI / 3);

    /**
    * \brief
    * The x-distance between two adjacent (on the x-axis) hexes.
    */
    public float CENTER_DISTANCE_X =>
      OUTER_HALF_WIDTH + TOP_WIDTH;

    /**
    * \brief
    * The determinant of the matrix `H` that converts hex-space to
    * unity-space.
    */
    public float D =>
      INNER_HALF_WIDTH * (1 + Mathf.Cos(-Mathf.PI/3)) * INNER_HEIGHT;

    public Vector2[] Corners {
      get {
        var self = this;
        return Enumerable.Range(0, 6)
          .Select(i => -i * Mathf.PI / 3)
          .Select(t => new Vector2(self.Ecos(t), self.Esin(t)))
          .ToArray();
      }
    }

    /**
    * \brief
    * Computes a sine on the ellipse associated with this hex.
    */
    public float Esin(float theta) {
      return Mathf.Sin(theta) * OUTER_HALF_HEIGHT;
    }

    /**
    * \brief
    * Computes a cosine on the ellipse associated with this hex.
    */
    public float Ecos(float theta) {
      return Mathf.Cos(theta) * OUTER_HALF_WIDTH;
    }

    public Vector2 Bounds(int width, int height) {
      // idea: pretend like the hexagons are arranged one next to the
      // other (not with nice offsetting)
      float w = width * INNER_WIDTH;
      // now because we have to squish the hexagons, we have to account
      // for the overlapping regions
      w -= (width - 1) * (OUTER_HALF_WIDTH - TOP_WIDTH);
      // account for the possibility that width is zero, which would
      // cause the product with (width - 1) to give a negative value of
      // w.
      if(w < 0)
        w = 0;

      // there's no interlocking in the y dimension, so we don't need to
      // subtract for overlapping regions.
      float h = height * INNER_HEIGHT;
      // however, if width > 1, then offsetting will occur, so we need
      // to bump up the height by one half-height.
      if(width > 1)
        h += INNER_HALF_HEIGHT;

      return new Vector2(w, h);
    }

    /**
    * \brief
    * Determines whether the given point is enclosed within a hexagon at the origin.
    */
    public bool Contains(Vector2 p) {
      // We traverse all the corners of the basic hexagon and check that
      // the point we're verifying lies on the RHS of each segment.
      // (We can check this using a cross product.)
      var corners = Corners;
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
        var z = Vector3.Cross(q.Upgrade(), s.Upgrade()).z;

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
  }
}
