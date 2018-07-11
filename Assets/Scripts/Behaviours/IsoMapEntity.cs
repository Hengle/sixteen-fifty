using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  public class IsoMapEntity : MonoBehaviour, INotifyDirectionChange {
    public event Action<HexDirection> DirectionChanged;
  }
}
