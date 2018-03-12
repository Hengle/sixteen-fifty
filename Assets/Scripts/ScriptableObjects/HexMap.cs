using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A map.
 * The event associated with map load can be used to set up the map
 * after its HexCells have been created in the HexGrid.
 */
[CreateAssetMenu(menuName = "1650/Hex Map")]
public class HexMap : ScriptableObject {
  public EventScript mapLoad;
  public int width;
  /* In offset coordinates. */
  public int initialPlayerX, initialPlayerY;
  public NPCSettings[] npcs;
  public int height => tiles.Length / width;
  public HexTile[] tiles;
}
