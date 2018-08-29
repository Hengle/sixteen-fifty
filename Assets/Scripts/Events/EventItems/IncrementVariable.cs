using System;

namespace SixteenFifty.EventItems {
  using Variables;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Increment Variable")]
  public class IncrementVariable : ImmediateScript, IEquatable<IncrementVariable> {
    public Variable<int> target;

    public override void Call(EventRunner runner) => target.Value++;

    public bool Equals(IncrementVariable that) =>
      target == that.target;

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
