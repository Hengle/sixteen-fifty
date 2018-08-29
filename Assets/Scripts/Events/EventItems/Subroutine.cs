using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.EventItems {
  using Commands;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Other script")]
  public class Subroutine : IScript, IEquatable<Subroutine> {
    public BasicScriptedEvent target;

    /**
     * \brief
     * Executes the `target` scripted event, if it exists.
     * Otherwise, does nothing.
     */
    public Command<object> GetScript(EventRunner runner) =>
      target?.Compile()?.GetScript(runner) ?? Command<object>.Empty;

    public bool Equals(Subroutine that) =>
      EqualityComparer<BasicScriptedEvent>.Default.Equals(
        target, that.target);

    public bool Equals(IScript _that) {
      var that = _that as Subroutine;
      return null != that && Equals(that);
    }
  }
}
