using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems {
  using Commands;

  [Serializable]
  [EventAttribute(friendlyName = "Event Sequence")]
  public class ListScript : IScript {
    public List<IScript> scripts;

    public Command<object> GetScript(EventRunner runner) {
      var cmd = Command<object>.Empty;
      foreach(var s in scripts) {
        cmd = cmd.Then(_ => s.GetScript(runner));
      }
      return cmd;
    }
  }
}
