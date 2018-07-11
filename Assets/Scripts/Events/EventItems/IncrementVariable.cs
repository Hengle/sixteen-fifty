using System;

namespace SixteenFifty.EventItems {
  using Variables;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Increment Variable")]
  public class IncrementVariable : ImmediateScript {
    public Variable<int> target;

    public override void Call(EventRunner runner) => target.Value++;
  }
}
