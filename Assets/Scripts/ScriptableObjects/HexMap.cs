using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.TileMap {
  /**
  * A map.
  * The event associated with map load can be used to set up the map
  * after its HexCells have been created in the HexGrid.
  */
  [CreateAssetMenu(menuName = "1650/Hex Map")]
  public class HexMap : ScriptableObject {
    /**
    * \brief
    * The metrics of the hexagons used in this hex map.
    */
    public HexMetrics metrics;

    /**
    * \brief
    * The width of the map.
    */
    public int width;

    /* In offset coordinates. */
    public int initialPlayerX, initialPlayerY;

    /**
    * \brief
    * Specifications of all the NPCs in the map.
    */
    public NPCSettings[] npcs;

    /**
    * \brief
    * The height of the map, computed by dividing the tile array length
    * by the map width.
    */
    public int height => tiles.Length / width;

    /**
    * \brief
    * Checks whether the map is rectangular.
    * All maps *must* be rectangular.
    */
    public bool isRectangular => height * width == tiles.Length;

    /**
    * \brief
    * The tile data.
    */
    public HexTile[] tiles;

    /**
    * \brief
    * Converts an index into the #tiles array into a pair of offset coordinates.
    */
    public void IndexToOffset(int i, out int x, out int y) {
      y = i / width;
      x = i % width;
    }

    /**
    * \brief
    * Converts a pair of offset coordinates into an index into the
    * #tiles array.
    */
    public int OffsetToIndex(int x, int y) => y * width + x;

    /**
    * \brief
    * Converts a tuple of offset coordinates (x, y) into an index into
    * the #tiles array.
    */
    public int OffsetToIndex(Tuple<int, int> t) => OffsetToIndex(t.Item1, t.Item2);

    /**
    * \brief
    * Gets or sets the HexTile at the given offset coordinates
    * represented as a tuple of integers.
    */
    public HexTile this[Tuple<int, int> t] {
      get {
        return this[t.Item1, t.Item2];
      }
      set {
        this[t.Item1, t.Item2] = value;
      }
    }

    /**
    * \brief
    * Gets or sets a HexTile object by offset coordinates.
    *
    * The setter will raise #TileChanged.
    */
    public HexTile this[int x, int y] {
      get {
        var i = OffsetToIndex(x, y);
        return this[i];
      }
      set {
        var i = OffsetToIndex(x, y);
        this[i] = value;
      }
    }

    /**
    * \brief
    * Gets or sets the tile at the given index.
    *
    * The setter will raise #TileChanged.
    */
    public HexTile this[int i] {
      get {
        return tiles[i];
      }
      set {
        tiles[i] = value;
        TileChanged?.Invoke(i, value);
      }
    }

    /**
    * \brief
    * Raised when a tile changes in the map.
    *
    * The integer is the index of the tile in #tiles.
    */
    public event Action<int, HexTile> TileChanged;

    /**
     * \brief
     * Clears all subscriptions to #TileChanged.
     *
     * This should only be used when the map is not in use.
     */
    public void ClearTileChanged() {
      if(null == TileChanged)
        return;
      Debug.Log(
        String.Format(
          "Cleared {0} subscriptions to TileChanged for HexMap {1}",
          TileChanged.GetInvocationList().Length,
          name));
      TileChanged = null;
    }
  }
}
