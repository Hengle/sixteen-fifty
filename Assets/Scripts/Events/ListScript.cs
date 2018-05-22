using System;
using System.Collections.Generic;

using Commands;

[Serializable]
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
