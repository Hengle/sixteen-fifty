using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/Example Map Load")]
public class ExampleEventScript : EventScript {
  public SpeakerData talkingDude;

  private EventManager manager;
  
  public static readonly HexCoordinates topRightCorner = new HexCoordinates(3, 4);

  public override Command<object> GetScript(EventRunner runner) {
    manager = runner.Manager;
    
    var t = manager.Canvas.GetComponent<RectTransform>();
    Speaker s = null;
    return Command<object>.Action(
      () => {

        manager.BlocksRaycasts = true;
      })
      .Then(
        _ => Command<Speaker>.Pure(
          () => s = Speaker.Construct(
            manager.speakerPrefab,
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
            "Click to move around!",
            "If you visit the top right corner, another event will play.",
          }))
      .Then(_ => new FadeSpeaker(runner, s, FadeDirection.OUT))
      .Then(
        _ => Command<object>.Action(() => GameObject.Destroy(s.gameObject)))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .Then(_ => Command<object>.Action(() => { runner.Manager.BlocksRaycasts = false; }));
  }
}
