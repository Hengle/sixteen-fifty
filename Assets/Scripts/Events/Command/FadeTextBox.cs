using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  public enum FadeDirection { IN, OUT }
  
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

    private Tuple<float, int> InitialAlphaAndMultiplier {
      get {
        if(direction == FadeDirection.IN) {
          return Tuple.Create(0f, 1);
        }
        else if(direction == FadeDirection.OUT) {
          return Tuple.Create(1f, -1);
        }
        else {
          throw new Exception("FadeDirection exhaustively analyzed");
        }
      }
    }
  
    public override IEnumerator GetCoroutine() {
      EventManager emgr = runner.Manager;

      emgr.TextBox.gameObject.SetActive(true);
      var renderer = emgr.TextBox.GetComponent<CanvasRenderer>();

      var t = InitialAlphaAndMultiplier;
      var initialAlpha = t.Item1;
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
      var delta = multiplier * 1f / iters;

      var alpha = initialAlpha;
      for(var i = 0; i < iters; i++) {
        renderer.SetAlpha(alpha);
        alpha += delta;
        yield return null;
      }

      // this works out to 1 if we started at zero, and vice versa.
      renderer.SetAlpha(1 - initialAlpha);

      result = null;
    }
  }
}
