using System;
using UnityEngine;

[Serializable]
public struct HexBox {
  [SerializeField]
  float centerX;

  [SerializeField]
  float centerY;

  [SerializeField]
  HexMetrics metrics;

  /**
   * \brief
   * Constructs a HexBox with the given HexMetrics for a position
   * given in offset coordinates.
   */
  public HexBox(int x, int y, HexMetrics metrics) {
    /*
     * Converts from hex coordinates to world-space coordinates.
     *
     * The math here is pretty hairy, but the derivation is not too bad.
     * We say that the x-axis in hex-space increases as we move southeast.
     * We can cook up a matrix that describes how to transform a
     * hex-point into a cartesian point.
     * The y-axis is trivial since an increment of 1 in the y direction
     * in hex-space corresponds to an increment of 2b' in cartesian
     * space, where b' = INNER_HEIGHT.
     * The x-axis is where the tricky stuff happens. To figure out what
     * direction we're moving in, it suffices to average the positions
     * of the first and last corner of the hex. This direction vector
     * isn't quite long enough though; it only takes us to the edge of
     * our hex. So we double it to obtain a vector that takes us to the
     * center of the next hex in the x-axis.
     * Then, we can arrange these vectors into a matrix as columns to
     * obtain a linear map from hex-space to pixel-space.
     * The math in this function is an inlined matrix multiplication by
     * this matrix, which we call H.
     */

    // we only do the coordinate conversion if a hexmetrics object is
    // really given to us.
    // In the editor we get null, and this completely breaks the
    // inspector for scripts that construct HexBox objects,
    // e.g. HexCell.
    if(metrics == null) {
      centerX = 0;
      centerY = 0;
    }
    else {
      centerX = x * (metrics.OUTER_HALF_WIDTH + metrics.Ecos(-Mathf.PI / 3));
      centerY = x * metrics.Esin(-Mathf.PI/3) + y * metrics.INNER_HEIGHT;
    }

    this.metrics = metrics;
  }

  /**
   * Determines whether this HexBox contains the given point.
   */
  public bool Contains(Vector2 position) {
    // to make the calculations simpler, we center everything:
    // we move the position to check to the origin.
    var p = position - Center;

    // because we're at the origin now, we can use the math from
    // HexMetrics to do the heavy lifting.
    return metrics.Contains(p);
  }

  public Vector2 Center =>
    new Vector2(centerX, centerY);

  public Vector2 TopLeft =>
    new Vector2(centerX - metrics.INNER_HALF_WIDTH, centerY + metrics.INNER_HALF_HEIGHT);

  public Vector2 TopCenter =>
    new Vector2(centerX, centerY + metrics.INNER_HALF_HEIGHT);

  public Vector2 TopRight =>
    new Vector2(centerX + metrics.INNER_HALF_WIDTH, centerY + metrics.INNER_HALF_HEIGHT);

  public Vector2 MiddleLeft =>
    new Vector2(centerX - metrics.INNER_HALF_WIDTH, centerY);

  public Vector2 MiddleCenter => Center;

  public Vector2 MiddleRight =>
    new Vector2(centerX + metrics.INNER_HALF_WIDTH, centerY);

  public Vector2 BottomLeft =>
    new Vector2(centerX - metrics.INNER_HALF_WIDTH, centerY - metrics.INNER_HALF_HEIGHT);

  public Vector2 BottomCenter =>
    new Vector2(centerX, centerY - metrics.INNER_HALF_HEIGHT);

  public Vector2 BottomRight =>
    new Vector2(centerX + metrics.INNER_HALF_WIDTH, centerY - metrics.INNER_HALF_HEIGHT);
}
