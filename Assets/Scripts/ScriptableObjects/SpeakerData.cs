using System;

using UnityEngine;

namespace SixteenFifty {
  /**
   * The properties of a talking head in cutscenes.
   */
  [CreateAssetMenu(menuName = "1650/Dialogue/Speaker")]
  public class SpeakerData : ScriptableObject, IEquatable<SpeakerData> {
    public Sprite sprite;

    public bool Equals(SpeakerData that) =>
      sprite?.Equals(that.sprite) ?? false;
  }

  [Serializable]
  public enum SpeakerOrientation { LEFT, RIGHT, }
}
