using UnityEngine;

namespace SixteenFifty {
  /**
   * \brief
   * Extension methods for Unity's `Sprite` class.
   */
  public static class SpriteExt {
    /**
     * \brief
     * Constructs a texture for a sprite.
     *
     * This method constructs a *brand new* Texture2D object each time,
     * which is a relatively expensive operation.
     */
    public static Texture2D GetCroppedTexture(this Sprite self) {
      var r = self.textureRect;
      var t = new Texture2D(
        (int)Mathf.Round(r.width),
        (int)Mathf.Round(r.height));
      var ps = self.texture.GetPixels(
        (int)Mathf.Round(r.x),
        (int)Mathf.Round(r.y),
        (int)Mathf.Round(r.width),
        (int)Mathf.Round(r.height));
      t.SetPixels(ps);
      t.Apply();
      return t;
    }
  }
}
