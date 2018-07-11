using System.Collections.Generic;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Implemented by MonoBehaviours that house a list of interactable
   * objects.
   */
  public interface IHasInteractables {
    List<Interactable> Interactables { get; }
  }
}
