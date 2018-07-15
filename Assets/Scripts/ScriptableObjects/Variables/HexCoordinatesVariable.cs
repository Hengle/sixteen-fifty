using UnityEngine;

namespace SixteenFifty.Variables {
  /**
   * \brief
   * A variable holding a HexCoordinates value.
   */
  [CreateAssetMenu(menuName = "1650/Variables/Hex Coordinates Variable")]
  public class HexCoordinatesVariable : PositionVariable<TileMap.HexCoordinates> {}
}
