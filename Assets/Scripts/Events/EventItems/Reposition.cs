using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems {
  using Variables;
  
  /**
   * \brief
   * Requests repositioning for the given position variable.
   */
  [Serializable]
  [SelectableSubtype(friendlyName = "Reposition")]
  public class Reposition : ImmediateScript, IEquatable<Reposition> {
    public IPositionVariable position;

    public override void Call(EventRunner runner) =>
      position.RequestReposition();

    public bool Equals(Reposition that) =>
      EqualityComparer<IPositionVariable>.Default.Equals(
        position, that.position);

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
