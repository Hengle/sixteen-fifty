using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using Variables;
  
  /**
  * An NPC with multiple interactions attached.
  */
  [CreateAssetMenu(menuName = "1650/Basic NPC")]
  public class BasicNPC : ScriptableObject {
    public HexMapEntity mapSprite;
    public HexDirection orientation;
    public HexCoordinatesVariable destination;
    public Interaction[] interactions;
  }
}
