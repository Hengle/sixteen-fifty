using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1650/Hex Map")]
public class HexMap : ScriptableObject {
  public EventScript mapLoad;
  public int width;
  public HexTile[] tiles;
  public int height => tiles.Length / width;
}
