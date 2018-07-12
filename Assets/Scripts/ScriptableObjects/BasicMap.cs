using UnityEngine;

namespace SixteenFifty {
  public class BasicMap : ScriptableObject {
    /**
     * \brief
     * The prefab used to construct this map.
     */
    public GameObject prefab;

    /**
     * \brief
     * The color with which to clear the camera.
     */
    public Color backgroundColor;
  }
}
