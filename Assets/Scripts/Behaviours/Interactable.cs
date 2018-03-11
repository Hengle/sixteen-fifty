using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Things that the player can interact with in the map.
 */
[RequireComponent(typeof(SpriteRenderer), typeof(MapEntity))]
public class Interactable : MonoBehaviour {
  public BasicNPC npcData;

  new SpriteRenderer renderer;
  
	// Use this for initialization
	void Awake () {
    renderer = this.GetComponentNotNull<SpriteRenderer>();
    renderer.sprite = npcData.mapSprite;
	}
}
