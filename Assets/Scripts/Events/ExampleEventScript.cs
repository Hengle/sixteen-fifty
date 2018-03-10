using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/Example Map Load")]
public class ExampleEventScript : EventScript {
  public SpeakerData talkingDude;
  
  public override Command<object> GetScript(EventRunner runner) {
    var t = runner.Manager.Canvas.GetComponent<RectTransform>();
    Speaker s = null;
    return Command<object>.Action(() => runner.Manager.BlocksRaycasts = true)
      .Then(
        _ => Command<Speaker>.Pure(
          () => s = Speaker.Construct(
            runner.Manager.speakerPrefab,
            talkingDude,
            0.3f,
            t,
            SpeakerOrientation.RIGHT)))
      .Then(speaker => new FadeSpeaker(runner, speaker, FadeDirection.IN))
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
      .Then(_ => new FadeSpeaker(runner, s, FadeDirection.OUT))
      .Then(
        _ => Command<object>.Action(() => GameObject.Destroy(s.gameObject)))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .Then(_ => Command<object>.Action(() => { runner.Manager.BlocksRaycasts = false; }));
  }
}
