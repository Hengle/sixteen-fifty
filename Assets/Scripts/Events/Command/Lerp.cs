using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SixteenFifty {
  namespace Commands {
    /**
    * Linearly interpolates between two values over a particular time,
    * executing a given action for each value.
    */
    public class Lerp : Command<object> {
      public const float DEFAULT_FADE_TIME = 0.2f;
      Action<float> action;
      float initial, final, time;

      public Lerp(float initial, float final, float time, Action<float> action = null) {
        this.action = action;
        this.initial = initial;
        this.final = final;
        this.time = time;
      }

      public Lerp(FadeDirection direction, float time, Action<float> action = null) {
        var t = direction.GetInitialValueAndMultiplier();
        this.initial = t.Item1;
        this.final = 1 - initial;
        this.time = time;
        this.action = action;
      }

      public Lerp WithAction(Action<float> action) {
        this.action = action;
        return this;
      }

      public override IEnumerator GetCoroutine() {
        var t = 0f;
        while(t < time) {
          action(Mathf.Lerp(initial, final, t / time));
          t += Time.deltaTime;
          yield return null;
        }

        action(final);

        yield break;
      }

      public static Lerp FadeImage(Image image, FadeDirection direction, float fadeTime = DEFAULT_FADE_TIME) {
        return new Lerp(
          direction,
          fadeTime,
          x => {
            var col = image.color;
            col.a = x;
            image.color = col;
          });
      }

      public static Lerp FadeSpriteRenderer(SpriteRenderer renderer, FadeDirection direction, float fadeTime = DEFAULT_FADE_TIME) {
        return new Lerp(
          direction,
          fadeTime,
          x => {
            var col = renderer.color;
            col.a = x;
            renderer.color = col;
          });
      }
    }
  }
}
