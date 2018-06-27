using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty {
  namespace Commands {
    public class ShowText : Command<object> {
      private string text;
      private EventRunner runner;
      private bool clicked;

      public const float CHARACTER_TIME = 0.04f;

      public static ShowText Clear(EventRunner runner) {
        return new ShowText(runner, "");
      }

      public ShowText(EventRunner runner, string text) {
        this.text = text;
        this.runner = runner;
        clicked = false;
      }

      private void OnClick(PointerEventData data) {
        clicked = true;
      }

      public override IEnumerator GetCoroutine() {
        EventManager emgr = runner.Manager;

        emgr.MainPanelClicked += OnClick;

        emgr.dialogueText.text = "";

        foreach(char c in text.ToCharArray()) {
          if(clicked)
            break;
          emgr.dialogueText.text += c;
          yield return new WaitForSeconds(CHARACTER_TIME);
        }

        emgr.dialogueText.text = text;

        emgr.MainPanelClicked -= OnClick;

        yield break;
      }
    }
  }
}
