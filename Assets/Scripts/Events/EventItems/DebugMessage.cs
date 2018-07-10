using System;

using UnityEngine;

namespace SixteenFifty.EventItems {
  [Serializable]
  [SelectableSubtype(friendlyName = "Debug Message")]
  public class DebugMessage : ImmediateScript {
    public string message;

    public override void Call(EventRunner runner) {
      Debug.Log(message);
    }
  }
}
