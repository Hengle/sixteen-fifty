using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/Simple Dialogue")]
public class SimpleDialogue : EventScript {
  public SpeakerData speaker;
  public string[] messages;
  public SpeakerOrientation orientation;

  public override Command<object> GetScript(EventRunner runner) {
    var manager = runner.Manager;

    var t = manager.Canvas.GetComponent<RectTransform>();
    Speaker s = null;
    return Command<object>.Action(() => manager.BlocksRaycasts = true)
      .ThenPure(
        _ => s = Speaker.Construct(
          manager.speakerPrefab,
          speaker,
          0.3f,
          t,
          orientation))
      .Then(speaker => new FadeSpeaker(runner, s, FadeDirection.IN))
      .Then(_ => new FadeTextBox(runner, FadeDirection.IN))
      .Then(_ => new Paragraph(runner, messages))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .Then(_ => new FadeSpeaker(runner, s, FadeDirection.OUT))
      .ThenAction(_ => GameObject.Destroy(s.gameObject))
      .ThenAction(_ => manager.BlocksRaycasts = false);
  }
}
