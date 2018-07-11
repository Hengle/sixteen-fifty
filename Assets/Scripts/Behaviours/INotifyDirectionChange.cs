using System;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Implemented by MonoBehaviour classes that can broadcast direction
   * change events.
   */
  public interface INotifyDirectionChange {
    event Action<HexDirection> DirectionChanged;
  }
}
