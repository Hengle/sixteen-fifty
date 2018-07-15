using System;

using UnityEngine;

namespace SixteenFifty.Variables {
  public class PositionVariable<T> : Variable<T>, IPositionVariable {
    /**
     * \brief
     * Raised when #RequestReposition is called.
     *
     * IPositioner objects can monitor the position variable using
     * this event, which is more specific than the basic #Changed
     * event from Variable<T>.
     */
    public event Action RepositionRequested;

    /**
     * \brief
     * Requests repositioning of the object holding this variable.
     *
     * In particular, this raises #RepositionRequested, which
     * IPositioner objects can listen for to perform their
     * repositioning logic.
     * Scripted events and other code can simply use
     * this method to warp the object in whatever way is appropriate
     * without worrying about the positioning logic.
     *
     * \sa
     * IPositioner, HexPositioner, IsoPositioner, MapEntity.
     */
    public void RequestReposition() =>
      RepositionRequested?.Invoke();
  }
}
