using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Things that the player can interact with in the map.
 */
[RequireComponent(typeof(SpriteRenderer), typeof(MapEntity), typeof(MapOrientation))]
public class Interactable : MonoBehaviour {
  public BasicNPC npcData;
}
