using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1650/Hex Tile")]
public class HexTile : ScriptableObject {
  public Sprite sprite;
  public int movementCost;
  public string tileName;
}
