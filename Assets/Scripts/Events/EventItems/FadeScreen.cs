using System;

namespace SixteenFifty.EventItems {
  using Commands;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Fade Screen")]
  public class FadeScreen : IScript, IEquatable<FadeScreen> {
    public FadeDirection direction;

    public Command<object> GetScript(EventRunner runner) =>
      Lerp.FadeImage(runner.Manager.fadeToBlackPanel, direction);

    public bool Equals(FadeScreen that) =>
      direction == that.direction;

    public bool Equals(IScript _that) {
      var that = _that as FadeScreen;
      return null != that && Equals(that);
    }
  }
}
