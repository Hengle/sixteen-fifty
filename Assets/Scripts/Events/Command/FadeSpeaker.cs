using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  public class FadeSpeaker : Command<Speaker> {
    public const float DEFAULT_FADE_TIME = 0.3f;

    EventRunner runner;
    Speaker speaker;
    FadeDirection direction;
    float fadeTime;

    public FadeSpeaker(
      EventRunner runner,
      Speaker speaker,
      FadeDirection direction,
      float fadeTime = FadeSpeaker.DEFAULT_FADE_TIME) {

      this.runner = runner;
      this.speaker = speaker;
      this.fadeTime = fadeTime;
      this.direction = direction;
    }
    
    public override IEnumerator GetCoroutine() {
      var t = direction.GetInitialValueAndMultiplier();
      var initialAlpha = t.Item1;
      var finalAlpha = 1 - initialAlpha;
      var multiplier = t.Item2;

      var renderer = speaker.GetComponent<SpriteRenderer>();

      // how many iterations until the fade is complete?
      var iters = (int)(fadeTime / Time.deltaTime);
      var col = renderer.color;
      for(int i = 0; i < iters; i++) {
        col.a = Mathf.Lerp(initialAlpha, finalAlpha, i / (float)iters);
        renderer.color = col;
        yield return null;
      }

      col.a = finalAlpha;
      renderer.color = col;
      
      result = speaker;
      yield break;
    }
  }
}
