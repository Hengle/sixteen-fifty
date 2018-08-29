using System;

namespace SixteenFifty.Variables {
  /**
   * \brief
   * Common features of all position variables.
   *
   * Two position variables are equal if they are the same variable.
   */
  public interface IPositionVariable : IEquatable<IPositionVariable> {
    event Action RepositionRequested;
    void RequestReposition();
  }
}
