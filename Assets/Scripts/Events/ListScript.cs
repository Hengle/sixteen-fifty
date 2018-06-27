using System;
using System.Collections.Generic;

namespace SixteenFifty {
  using Commands;

  [EventAttribute(friendlyName = "Event Sequence")]
  public class ListScript : EventScript {
    public List<EventScript> scripts;

    public override Command<object> GetScript(EventRunner runner) {
      var cmd = Command<object>.Empty;
      foreach(var s in scripts) {
        cmd = cmd.Then(_ => s.GetScript(runner));
      }
      return cmd;
    }
  }
}
