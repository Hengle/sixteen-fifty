using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

[CreateAssetMenu(menuName = "1650/Events/Example Map Load")]
public class ExampleEventScript : EventScript {
  public SpeakerData talkingDude;
  public EventScript jakeDialogue;

  private EventManager manager;
  private HexGrid map;
  
  public static readonly HexCoordinates topRightCorner = new HexCoordinates(3, 4);

  public override Command<object> GetScript(EventRunner runner) {
    this.manager = runner.Manager;
    this.map = runner.Map;
    
    var t = manager.Canvas.GetComponent<RectTransform>();
    Speaker s = null;
    return Command<object>.Action(() => manager.BlocksRaycasts = true)
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
      .Then(_ => ShowText.Clear(runner))
      .Then(_ => new FadeSpeaker(runner, s, FadeDirection.OUT))
      .Then(
        _ => Command<object>.Action(() => GameObject.Destroy(s.gameObject)))
      .Then(_ => new FadeTextBox(runner, FadeDirection.OUT))
      .ThenAction(
        _ => {
          var me = map.GetComponentInParentNotNull<TestLevel>().player.GetComponent<MapEntity>();
          me.EndMove += OnEndMoveInCorner;
        })
      .Then(_ => Command<object>.Action(() => { runner.Manager.BlocksRaycasts = false; }));
  }

  private void OnEndMoveInCorner(MapEntity me) {
    if(!me.CurrentCell.coordinates.Equals(topRightCorner))
      return;

    me.EndMove -= OnEndMoveInCorner;
    manager.BeginScript(map, jakeDialogue);
  }
}
