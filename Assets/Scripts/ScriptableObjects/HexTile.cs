using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  [CreateAssetMenu(menuName = "1650/Hex Tile")]
  public class HexTile : ScriptableObject {
    public Sprite sprite;
    public int movementCost;
    public string tileName;

    /**
    * \brief
    * Interactions that are possible with this type of tile.
    */
    public Interaction[] interactions;
  }
}
