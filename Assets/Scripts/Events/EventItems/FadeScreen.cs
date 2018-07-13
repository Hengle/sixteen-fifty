using System;

namespace SixteenFifty.EventItems {
  using Commands;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Fade Screen")]
  public class FadeScreen : IScript {
    public FadeDirection direction;

    public Command<object> GetScript(EventRunner runner) =>
      Lerp.FadeImage(runner.Manager.fadeToBlackPanel, direction);
  }
}
