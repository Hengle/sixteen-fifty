using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;

  [Serializable]
  [SelectableSubtype(friendlyName = "Event Sequence")]
  public class ListScript : IScript, IEquatable<ListScript> {
    public List<IScript> scripts;

    public Command<object> GetScript(EventRunner runner) {
      var cmd = Command<object>.Empty;
      foreach(var s in scripts) {
        if(null == s)
          Debug.LogWarning(
            "Element of event sequence is null.");
        else
          cmd = cmd.Then(_ => s.GetScript(runner));
      }
      return cmd;
    }

    public bool Equals(ListScript that) =>
      scripts?.SequenceEqual(that.scripts) ?? false;

    public bool Equals(IScript _that) {
      var that = _that as ListScript;
      return null != that && Equals(that);
    }
  }
}
