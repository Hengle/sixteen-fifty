using System;

namespace SixteenFifty.EventItems {
  using Variables;
  
  /**
   * \brief
   * Requests repositioning for the given position variable.
   */
  [Serializable]
  [SelectableSubtype(friendlyName = "Reposition")]
  public class Reposition : ImmediateScript {
    public IPositionVariable position;

    public override void Call(EventRunner runner) =>
      position.RequestReposition();
  }
}
