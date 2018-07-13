using System;

namespace SixteenFifty.Behaviours {
  public interface IPositioner {
    /**
     * \brief
     * Fires right after the entity's initial position is set.
     */
    event Action Positioned;
  }
}
