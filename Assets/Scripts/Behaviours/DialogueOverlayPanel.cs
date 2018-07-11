using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  /**
  * \brief
  * An invisible panel used to capture clicks to make dialogues
  * progress.
  */
  public class DialogueOverlayPanel : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData data) {
      eventManager.RaiseMainPanelClicked(data);
    }

    public EventManager eventManager;
  }
}
