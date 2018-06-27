using System;
using UnityEngine;

namespace SixteenFifty {
  using Commands;

  /**
   * \brief
   * A `ScriptableObject` that's an IScript.
   */
  [Serializable]
  public abstract class EventScript : ScriptableObject, IScript {
    public abstract Command<object> GetScript(EventRunner runner);
  }
}
