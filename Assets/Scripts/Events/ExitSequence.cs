using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/Exit Sequence")]
public class ExitSequence : EventScript {
  public override Command<object> GetScript(EventRunner runner) {
    var manager = runner.Manager;
    return Command<object>
      .Action(() => manager.BlocksRaycasts = true)
      .Then(
        _ => new Lerp(0f, 1f, 2.5f)
        .WithAction(
          x => {
            var col = manager.SecretPanel.color;
            col.a = x;
            manager.SecretPanel.color = col;
          }));
  }
}
