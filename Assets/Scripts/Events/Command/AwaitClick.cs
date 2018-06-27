using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty {
  namespace Commands {
    /**
    * \brief
    * A command that completes when the EventOverlayPanel is clicked.
    */
    public class AwaitClick : Command<object> {
      private EventRunner runner;
      private bool clicked;

      public AwaitClick(EventRunner runner) {
        this.runner = runner;
        clicked = false;
      }

      private void OnClick(PointerEventData data) {
        clicked = true;
      }

      public override IEnumerator GetCoroutine() {
        runner.Manager.MainPanelClicked += OnClick;
        while(!clicked) yield return null;
        runner.Manager.MainPanelClicked -= OnClick;
        yield break;
      }
    }
  }
}
