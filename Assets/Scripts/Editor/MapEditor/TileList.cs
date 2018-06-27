using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty {
  namespace Editor {
    /**
    * \brief
    * Manages the list of tiles to be displayed in the map editor.
    */
    public class TileList : IDataSet<TileButton> {
      private TileButton[] tiles;

      public int Count => tiles?.Length ?? 0;

      IEnumerator IEnumerable.GetEnumerator() {
        if(null == tiles)
          return Enumerable.Empty<TileButton>().GetEnumerator() as IEnumerator;
        return tiles.AsEnumerable().GetEnumerator();
      }

      public IEnumerator<TileButton> GetEnumerator() {
        if(null == tiles)
          return Enumerable.Empty<TileButton>().GetEnumerator();
        return tiles.AsEnumerable<TileButton>().GetEnumerator();
      }

      /**
        * \brief
        * Releases temporary resources required to maintain the tile list.
        */
      private void Release() {
        Debug.Assert(null != tiles);
        foreach(var tile in tiles) {
          tile.Dispose();
        }
        tiles = null;
      }

      /**
        * \brief
        * Refreshes the list of tiles available for map editing.
        *
        * If there is an existing list of available tiles, it is released
        * by calling #ReleaseTileList.
        */
      public void Refresh() {
        if(null != tiles)
          Release();

        tiles =
          EditorMisc.GetAllInstances<HexTile>()
          .Select(t => new TileButton(t))
          .ToArray();
      }

      public void Dispose() {
        if(null != tiles)
          Release();
      }
    }
  }
}
