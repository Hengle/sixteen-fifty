using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty {
  namespace Editor {
    /**
     * \brief
     * A button representing a tile, to be displayed in the map editor
     * window.
     *
     * This class manages the construction of a custom texture for the
     * button, so that sprites selected as a sub-region of a larger
     * image will appear correctly. Otherwise, merely using
     * HexTile#sprite's `texture` property will retrieve the whole
     * image that the sprite is taken from, and not just the part that
     * the sprite actually is.
     */
    public struct TileButton : IDisposable {
      public Texture2D texture;
      public AssetInfo<HexTile> assetInfo;
      
      public TileButton(AssetInfo<HexTile> assetInfo) {
        texture = assetInfo.asset.sprite.GetCroppedTexture();
        this.assetInfo = assetInfo;
      }
      
      public void Dispose() {
        UnityEngine.Object.DestroyImmediate(texture);
      }
    }
  }
}
