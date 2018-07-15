using System;

namespace SixteenFifty.Variables {
  /**
   * \brief
   * Common features of all position variables.
   */
  public interface IPositionVariable {
    event Action RepositionRequested;
    void RequestReposition();
  }
}
