using System;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Implemented by classes that are meant to position an object in
   * some kind of setting.
   *
   * Implementations typically reference a Variable<T> object that
   * holds the destination of the target.
   *
   * \sa
   * HexPositioner, IsoPositioner.
   */
  public interface IPositioner {
    /**
     * \brief
     * Fires right after the entity's initial position is set.
     */
    event Action Positioned;

    /**
     * \brief
     * Requests a repositioning of the object.
     */
    void Reposition();
  }
}
