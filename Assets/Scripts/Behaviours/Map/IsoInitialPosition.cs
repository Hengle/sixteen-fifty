using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Variables;

  public class IsoInitialPosition : MonoBehaviour {
    public Vector2Variable destination;

    void Start() {
      if(null == destination)
        return;
      
      transform.position = destination.Value;
    }
  }
}
