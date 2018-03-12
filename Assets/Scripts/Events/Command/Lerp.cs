using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Commands {
  /**
   * Linearly interpolates between two values over a particular time,
   * executing a given action for each value.
   */
  public class Lerp : Command<object> {
    Action<float> action;
    float initial, final, time;
  
    public Lerp(float initial, float final, float time, Action<float> action = null) {
      this.action = action;
      this.initial = initial;
      this.final = final;
      this.time = time;
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

      yield break;
    }
  }
}
