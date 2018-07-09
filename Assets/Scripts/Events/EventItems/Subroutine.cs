using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.EventItems {
  using Commands;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Other script")]
  public class Subroutine : IScript {
    public BasicScriptedEvent target;

    /**
     * \brief
     * Executes the `target` scripted event, if it exists.
     * Otherwise, does nothing.
     */
    public Command<object> GetScript(EventRunner runner) =>
      target?.Compile()?.GetScript(runner) ?? Command<object>.Empty;
  }
}
