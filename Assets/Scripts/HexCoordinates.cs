using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct HexCoordinates {

  public delegate int Transformation(int coord);

  [SerializeField]
  private int x, y;
  
  public int X => x;

  public int Y => y;
  
  public int Z => -X - Y;

  public HexCoordinates (int x, int y) {
    this.x = x;
    this.y = y;
  }

  public static HexCoordinates FromOffsetCoordinates(int x, int y)
  => new HexCoordinates(x, y + x / 2);

  public Tuple<int, int> ToOffsetCoordinates()
  => Tuple.Create(x, y - x/2);

  public HexCoordinates TransformX(Transformation f)
  => new HexCoordinates(f(X), Y);

  public HexCoordinates TransformY(Transformation f)
  => new HexCoordinates(X, f(Y));

  public HexCoordinates Transform(Transformation f, Transformation g)
  => new HexCoordinates(f(X), g(Y));

  public HexCoordinates TranslateX(int x)
  => TransformX(x_ => x_ + x);

  public HexCoordinates TranslateY(int y)
  => TransformY(y_ => y_ + y);

  public HexCoordinates TranslateZ(int z)
  => Transform(x => x - z, y => y + z);

  public HexCoordinates[] Neighbours
  // WARNING: the order of elements in this array matches the order of
  // the directions in the HexDirections.
  => new [] {
    TranslateY(1),
    TranslateZ(-1),
    TranslateX(1),
    TranslateY(-1),
    TranslateZ(1),
    TranslateX(-1),
  };

  /**
   * To allow writing @coord[North]@ to access the coordinates of the
   * northern neighbour.
   */
  public HexCoordinates this[HexDirection d] => Neighbours[(int)d];

  /**
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
  public Vector2 ToPosition()
  => new Vector2(
    // X * HexMetrics.OUTER_WIDTH * (1 + Math.Cos(-Math.PI / 3)),
    X * (HexMetrics.OUTER_WIDTH + (float) HexMetrics.Ecos(-Math.PI / 3)),
    // X * HexMetrics.OUTER_HEIGHT * Math.Sin(-Math.PI / 3) + Y * 2 * HexMetrics.OUTER_HEIGHT,
    X * (float) HexMetrics.Esin(-Math.PI/3) + 2 * Y * HexMetrics.INNER_HEIGHT);

  /**
   * By inverting H, we can convert from pixel-space into a fractional hex-coordinate.
   */
  public static HexCoordinates FromPosition(Vector2 position) {
    // multiply by the reciprocal determinant of H^-1.
    var p = position * (1 / HexMetrics.D);
    // the math here is the multiplication by the inverse matrix.
    // We round to obtain integer hex coordinates, but this rounding
    // sometimes produces off-by-one errors.
    var guess = new HexCoordinates(
      (int)Math.Round(p.x * 2 * HexMetrics.INNER_HEIGHT),
      (int)Math.Round(
        - p.x * (float) HexMetrics.Esin(-Math.PI/3)
        + p.y * HexMetrics.INNER_WIDTH * (1 + (float) Math.Cos(-Math.PI/3))));

    // to resolve the off-by-one error, it seems to be a standard
    // practice to refine this guess by looking at its neighbouring
    // hexes and looking for the one that *really* contains the point,
    // since computing whether a point lies within a convex polygon is
    // pretty fast.
    var e = new List<HexCoordinates>(new [] { guess });
    e.AddRange(guess.Neighbours);
    foreach(var h in e.Where(h => h.Contains(position))) {
      // as soon as we find a hex that contains p, we can return it:
      return h;
    }

    // if no hexes contain p, then we have reached an illegal state.
    throw new Exception("failed to locate point");
  }

  /**
   * Determines whether this hexagon contains the given point.
   */
  public bool Contains(Vector2 position) {
    // to make the calculations simpler, we center everything:
    // we move the position to check to the origin.
    var p = position - ToPosition();

    // because we're at the origin now, we can use the math from
    // HexMetrics to do the heavy lifting.
    return HexMetrics.Contains(p);
  }

  public override string ToString()
  => "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";

  public string ToStringMultiline()
  => X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
}
