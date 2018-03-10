using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName ="1650/Events/Jake Dialogue")]
public class JakeDialogueEvent : EventScript {
  public SpeakerData jakeData;

  public override Command<object> GetScript(EventRunner runner) {
    Speaker jake = null;
    var t = runner.Manager.Canvas.GetComponent<RectTransform>();
    return Command<object>.Action(() => runner.Manager.BlocksRaycasts = true)
      .ThenPure(
        _ => jake = Speaker.Construct(
          runner.Manager.speakerPrefab,
          jakeData,
          0.3f,
          t,
          SpeakerOrientation.RIGHT))
      .Then(speaker => new FadeSpeaker(runner, speaker, FadeDirection.IN))
      .Then(_ => new FadeTextBox(runner, FadeDirection.IN))
      .Then(
        _ => new Paragraph(
          runner,
          new [] {
            "Hi! My name is Jake, and I'm one of the developers of this game!",
            "Calling it a game right now might be a bit of a stretch, but you can get at least get a basic idea of the look and feel of our project.",
            "You should talk to by boyfriend Eric over there to see what he has to say about the game too!",
          }))
      .Then(_ => ShowText.Clear(runner))
      .Then(_ => new FadeSpeaker(runner, jake, FadeDirection.OUT))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .ThenPure<object>(
        _ => {
          runner.Manager.BlocksRaycasts = false;
          return null;
        });
  }

}
