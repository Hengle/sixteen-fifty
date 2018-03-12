using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/IntroSequence")]
public class IntroSequence : EventScript {
  public EventScript introDialogue;

  public override Command<object> GetScript(EventRunner runner) {
    var manager = runner.Manager;
    var s = manager.SecretPanel;
    var t = manager.TitleScreen;
    return Command<object>
      .Action(
        () => {
          manager.BlocksRaycasts = true;
          var col = t.color;
          col.a = 1;
          t.color = col;
          col = s.color;
          col.a = 1;
          s.color = col;
        })
      .Then(_ => new Delay(2f))
      .Then(
        _ => new Lerp(
          1, 0, 1f, x => {
            var col = t.color;
            col.a = x;
            t.color = col;
          }))
      .Then(_ => new Delay(1f))
      .Then(
        _ => new Lerp(
          1, 0, 1f, x => {
            var col = s.color;
            col.a = x;
            s.color = col;
          }))
      .ThenAction(_ => manager.BlocksRaycasts = false)
      .Then(_ => introDialogue.GetScript(runner));
  }
}
