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
      .Then(_ => Lerp.FadeImage(manager.fadeToBlackPanel, FadeDirection.IN, 2.5f))
      .Then(_ => new Delay(1f))
      .ThenAction(_ => Application.Quit());
  }
}
