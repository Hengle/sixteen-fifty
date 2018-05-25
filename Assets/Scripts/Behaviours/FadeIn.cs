using UnityEngine;

using Commands;

public class FadeIn : MonoBehaviour {
  CanvasGroup canvasGroup;

  public bool active = true;
  public float time = 0.1f;
  
  void Awake() {
    canvasGroup = GetComponentInParent<CanvasGroup>();
  }

  void OnEnable() {
    StartCoroutine(
      new Lerp(FadeDirection.IN, time, t => canvasGroup.alpha = t)
      .GetCoroutine());
  }
}
