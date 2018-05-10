using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  /**
     \brief
     Fades an InteractionMenu in or out.
   */ 
  public class FadeInteractionMenu : Command<object> {
    public const float DEFAULT_FADE_TIME = 0.35f;

    InteractionMenu menu;
    float fadeTime;
    FadeDirection direction;
  
    public FadeInteractionMenu(
      InteractionMenu menu,
      FadeDirection direction,
      float fadeTime = DEFAULT_FADE_TIME) {

      this.menu = menu;
      this.direction = direction;
      this.fadeTime = fadeTime;
    }
  
    public override IEnumerator GetCoroutine() {
      var t = direction.GetInitialValueAndMultiplier();
      var initialAlpha = t.Item1;
      var finalAlpha = 1- initialAlpha;
      var group = menu.GetComponentInChildrenNotNull<CanvasGroup>();
      var iters = (int)(fadeTime / Time.deltaTime);
      for(int i = 0; i <= iters; i++) {
        group.alpha = Mathf.Lerp(initialAlpha, finalAlpha, (float)i / iters);
        yield return null;
      }

      result = null;
    }
  }
}
