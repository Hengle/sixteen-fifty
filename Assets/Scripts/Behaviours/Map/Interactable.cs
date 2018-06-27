using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty.TileMap {
  /**
   * Things that the player character can interact with in the map.
   */
  [RequireComponent(typeof(SpriteRenderer), typeof(MapEntity), typeof(MapOrientation))]
  public class Interactable : MonoBehaviour {
    public BasicNPC npcData;
  }
}
