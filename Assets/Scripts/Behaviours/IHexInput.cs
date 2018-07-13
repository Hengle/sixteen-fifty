using System;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * An interface for behaviours that can produce action requests in a
   * hexagonal way.
   */
  public interface IHexInput {
    /**
     * \brief
     * Fires when a change in direction is requested.
     *
     * The given value is null when the stick is neutral.
     */
    event Action<Maybe<HexDirection>> DirectionChanged;

    /**
     * \brief
     * Fires on the frame the submit button is released.
     */
    event Action SubmitPressed;
  }
}
