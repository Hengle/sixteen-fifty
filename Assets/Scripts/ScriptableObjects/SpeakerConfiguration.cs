using System;

namespace SixteenFifty {
  [Serializable]
  public class SpeakerConfiguration : IEquatable<SpeakerConfiguration> {
    public SpeakerData speakerData;
    public float position;
    public SpeakerOrientation orientation;

    public bool Equals(SpeakerConfiguration that) =>
      speakerData.Equals(that.speakerData) &&
      position.Equals(that.position) &&
      orientation.Equals(that.orientation);
  }
}
