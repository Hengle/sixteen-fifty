using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  public class FadeTextBox : Command<object> {
    public const float DEFAULT_FADE_TIME = 0.25f;
    
    private float fadeTime;
    private EventRunner runner;
    private FadeDirection direction;

    public FadeTextBox(EventRunner runner, FadeDirection direction, float fadeTime = DEFAULT_FADE_TIME) {
      this.fadeTime = fadeTime;
      this.runner = runner;
      this.direction = direction;
    }

    public override IEnumerator GetCoroutine() {
      EventManager emgr = runner.Manager;

      var box = emgr.TextBox;

      var t = direction.GetInitialValueAndMultiplier();
      var initialAlpha = t.Item1;
      // this works out to 1 if we started at zero, and vice versa.
      var finalAlpha = 1 - initialAlpha;
      var multiplier = t.Item2;
      
      // we want to go from 0 to 1 (or 1 to 0) over the course of
      // fadeTime seconds.
      // how many iterations will this take?
      // each iteration takes Time.deltaTime, so we divide the
      // fadeTime by the time delta.
      var iters = (int)(fadeTime / Time.deltaTime);
      // the amount to change the alpha each iteration depends on what
      // direction we're moving in, so we multiply by the multiplier,
      // which acts to invert the direction.
      var delta = (float)multiplier / iters;

      var alpha = initialAlpha;
      var col = box.color;
      for(var i = 0; i < iters; i++) {
        col.a = alpha;
        box.color = col;
        alpha += delta;
        yield return null;
      }

      col.a = finalAlpha;
      box.color = col;

      result = null;
    }
  }
}
