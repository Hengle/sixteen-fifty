using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An NPC with multiple interactions attached.
 */
[CreateAssetMenu(menuName = "1650/Basic NPC")]
public class BasicNPC : ScriptableObject {
  public HexMapEntity mapSprite;
  public Interaction[] interactions;
  public HexDirection orientation;
}
