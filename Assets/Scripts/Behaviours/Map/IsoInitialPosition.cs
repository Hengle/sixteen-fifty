using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Variables;

  public class IsoInitialPosition : MonoBehaviour, IPositioner {
    public Vector2Variable destination;

    public event Action Positioned;

    void Start() {
      if(null == destination)
        return;
      
      transform.position = destination.Value;

      Positioned?.Invoke();
    }
  }
}
