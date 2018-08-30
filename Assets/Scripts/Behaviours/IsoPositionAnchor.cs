using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Variables;
  
  public class IsoPositionAnchor : MonoBehaviour {
    public Vector2Variable destination;

    void Start() {
      if(null == destination)
        return;
      destination.Value = transform.position.Downgrade();
    }
  }
}
