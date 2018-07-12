using System.Collections.Generic;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Implemented by MonoBehaviours that house a list of interactable
   * objects.
   */
  public interface IHasInteractables {
    IEnumerable<Interactable> Interactables { get; }

    void AddInteractable(Interactable interactable);
  }
}
