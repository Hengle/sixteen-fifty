using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

public class ExampleEventScript : EventScript {
  public Command<object> GetScript(EventRunner runner) {
    return Command<object>.Action(() => runner.Manager.BlocksRaycasts = true)
      .Then(_ => new FadeTextBox(runner, FadeDirection.IN))
      .Then(
        _ => new Paragraph(
          runner,
          new string[] {
            "Hello world!",
            "How's it going?",
            "This is our kickass game.",
            "I heard you really enjoy dialogue, so this is a much much longer one to test word wrapping!!!!"
          }))
      .Then(_ => ShowText.Clear(runner))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .Then(_ => Command<object>.Action(() => { runner.Manager.BlocksRaycasts = false; }));
  }
}
