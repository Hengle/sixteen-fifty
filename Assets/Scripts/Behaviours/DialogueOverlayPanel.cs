using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * \brief
 * An invisible panel used to capture clicks to make dialogues
 * progress.
 */
public class DialogueOverlayPanel : MonoBehaviour, IPointerClickHandler {
  public void OnPointerClick(PointerEventData data) {
    emgr.RaiseMainPanelClicked(data);
  }

  EventManager emgr;

  public void Awake() {
    emgr = this.GetComponentInParentNotNull<EventManager>();
  }
}
