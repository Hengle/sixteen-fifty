using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SixteenFifty.TileMap {
  [Serializable]
  public struct HexCoordinates {

    public delegate int Transformation(int coord);

    [SerializeField]
    HexMetrics metrics;

    [SerializeField]
    int x, y;

    public int X => x;

    public int Y => y;

    public int Z => -X - Y;

    public static int Distance(HexCoordinates a, HexCoordinates b) =>
      (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;

    public int DistanceTo(HexCoordinates dst) => Distance(this, dst);

    public HexCoordinates (int x, int y, HexMetrics metrics) {
      this.x = x;
      this.y = y;
      this.metrics = metrics;
      this.Box = new HexBox(x, y, metrics);
    }

    public static HexCoordinates Zero(HexMetrics metrics) =>
      new HexCoordinates(0, 0, metrics);

    public static HexCoordinates FromOffsetCoordinates(int x, int y, HexMetrics metrics)
    => new HexCoordinates(x, y + x / 2, metrics);

    public Tuple<int, int> ToOffsetCoordinates()
    => Tuple.Create(x, y - x/2);

    public HexCoordinates TransformX(Transformation f)
    => new HexCoordinates(f(X), Y, metrics);

    public HexCoordinates TransformY(Transformation f)
    => new HexCoordinates(X, f(Y), metrics);

    public HexCoordinates Transform(Transformation f, Transformation g)
    => new HexCoordinates(f(X), g(Y), metrics);

    public HexCoordinates TranslateX(int x)
    => TransformX(x_ => x_ + x);

    public HexCoordinates TranslateY(int y)
    => TransformY(y_ => y_ + y);

    public HexCoordinates TranslateZ(int z)
    => Transform(x => x - z, y => y - z);

    private static Func<HexCoordinates, HexCoordinates>[] NeighbourFunctions
    // WARNING: the order of elements in this array matches the order of
    // the directions in the HexDirections.
    => new Func<HexCoordinates, HexCoordinates>[] {
      o => o.TranslateY(1),
      o => o.TranslateZ(-1),
      o => o.TranslateX(1),
      o => o.TranslateY(-1),
      o => o.TranslateZ(1),
      o => o.TranslateX(-1),
    };

    public IEnumerable<HexCoordinates> Neighbours {
      get {
        var self = this;
        return HexCoordinates.NeighbourFunctions.Select(f => f(self));
      }
    }

    /**
    * To allow writing @coord[North]@ to access the coordinates of the
    * northern neighbour.
    */
    public HexCoordinates this[HexDirection d] => HexCoordinates.NeighbourFunctions[(int)d](this);

    /**
    * \brief
    * Gets the HexBox whose center is located at these hex coordinates.
    */
    public HexBox Box;

    /**
    * \brief
    * Determines which hex contains the given position.
    *
    * By inverting H, we can convert from pixel-space into a fractional
    * hex-coordinate.
    * This operation is relatively expensive because it involves
    * guessing what the hex is and then refining. The guess can be off
    * by at most one hex, so this still isn't so bad.
    *
    * \sa #Box
    */
    public static HexCoordinates FromPosition(Vector2 position, HexMetrics metrics) {
      // multiply by the reciprocal determinant of H^-1.
      var p = position * (1 / metrics.D);
      // the math here is the multiplication by the inverse matrix.
      // We round to obtain integer hex coordinates, but this rounding
      // sometimes produces off-by-one errors.
      var guess = new HexCoordinates(
        (int)Math.Round(p.x * metrics.INNER_HEIGHT),
        (int)Math.Round(
          - p.x * metrics.Esin(-Mathf.PI/3)
          + p.y * metrics.INNER_HALF_WIDTH * (1 + Mathf.Cos(-Mathf.PI/3))),
        metrics);

      // to resolve the off-by-one error, it seems to be a standard
      // practice to refine this guess by looking at its neighbouring
      // hexes and looking for the one that *really* contains the point,
      // since computing whether a point lies within a convex polygon is
      // pretty fast.
      var e = new List<HexCoordinates>(new [] { guess });
      e.AddRange(guess.Neighbours);
      foreach(var h in e.Where(h => h.Box.Contains(position))) {
        // as soon as we find a hex that contains p, we can return it:
        return h;
      }

      // if no hexes contain p, then we have reached an illegal state.
      throw new Exception("failed to locate point");
    }

    public override string ToString()
    => "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";

    public string ToStringMultiline()
    => X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();

    /**
    * Determines which neighbour the given coordinates are for the current hex.
    * Return null if the given coordinates do not neighbour this position.
    */
    public HexDirection? WhichNeighbour(HexCoordinates other) {
      var self = this;
      return Enumerable.Range(0, 6)
        .Select(i => (HexDirection)i)
        .Select(d => Tuple.Create(d, self[d]))
        .Where(t => t.Item2.Equals(other))
        .Select(t => t.Item1)
        .FirstOrDefault();
    }
  }
}
