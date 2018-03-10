using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventOverlayPanel : MonoBehaviour, IPointerClickHandler {
  public void OnPointerClick(PointerEventData data) {
    emgr.RaiseMainPanelClicked(data);
  }

  EventManager emgr;

  public void Awake() {
    emgr = this.GetComponentInParentNotNull<EventManager>();
  }
}
