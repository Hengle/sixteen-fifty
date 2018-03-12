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
      // display the title screen
      // and make the secret panel full black
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
      // wait two seconds, and then fade out the title screen
      .Then(_ => new Delay(2f))
      .Then(
        _ => new Lerp(
          1, 0, 1f, x => {
            var col = t.color;
            col.a = x;
            t.color = col;
          }))
      // wait one more second with the screen black
      .Then(_ => new Delay(1f))
      .Then(
        _ => new Lerp(
          1, 0, 1f, x => {
            var col = s.color;
            col.a = x;
            s.color = col;
          }))
      .ThenAction(_ => runner.Map.GetComponent<AudioSource>().Play())
      .Then(
        _ => {
          var player = GameObject.FindWithTag("Player");
          var target = player.transform.position;
          target.z = Camera.main.transform.position.z;
          return new MoveTransform(Camera.main.transform, target);
        })
      .ThenAction(_ => manager.BlocksRaycasts = false)
      .Then(_ => introDialogue.GetScript(runner));
  }
}
