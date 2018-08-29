using System;

using UnityEngine;

namespace SixteenFifty.EventItems {
  [Serializable]
  [SelectableSubtype(friendlyName = "Debug Message")]
  public class DebugMessage : ImmediateScript, IEquatable<DebugMessage> {
    public string message;

    public override void Call(EventRunner runner) {
      Debug.Log(message);
    }

    public bool Equals(DebugMessage that) =>
      message == that.message;

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
