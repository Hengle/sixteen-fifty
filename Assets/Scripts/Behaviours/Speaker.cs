using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SpeakerOrientation { LEFT, RIGHT, }

/**
 * Represents a talking head in an event script.
 */
public class Speaker : MonoBehaviour {
  /**
   * The position measures between 0 and 1 where from left to right
   * the speaker is positioned at the bottom of the given screenRect.
   */
  public static Speaker Construct(
    GameObject prefab,
    SpeakerData data,
    float position,
    RectTransform parent,
    SpeakerOrientation orientation = SpeakerOrientation.LEFT) {

    var obj = Instantiate(prefab);
    var s = obj.GetComponent<Speaker>();
    Debug.Assert(s != null);
    var renderer = obj.GetComponent<SpriteRenderer>();
    s.data = data;
    renderer.sprite = s.data.sprite;

    var screenRect = parent.rect;

    obj.transform.parent = parent;
    obj.transform.localPosition =
      new Vector2(
        screenRect.width * position - screenRect.width/2,
        -screenRect.yMax);

    if(orientation == SpeakerOrientation.RIGHT) {
      renderer.flipX = true;
    }

    return s;
  }

  public SpeakerData data;

  void Awake() {

  }
}
