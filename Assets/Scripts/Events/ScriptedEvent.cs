using System;
using System.Collections.Generic;

using UnityEngine;

using SixteenFifty.Serialization;

namespace SixteenFifty {
  using Commands;
  using EventItems;

  [CreateAssetMenu(menuName = "1650/Scripted Event")]
  public class ScriptedEvent : SerializableScriptableObject {
    new public string name;
    public IScript root;
  }

  /**
   * \brief
   * An immediate script just executes a function.
   *
   * Use this for event items that don't take place over time.
   */
  [Serializable]
  public abstract class ImmediateScript : IScript {
    public Command<object> GetScript(EventRunner runner) {
      return
        Command<EventRunner>.Pure(() => runner)
        .ThenAction(Call);
    }

    public abstract void Call(EventRunner runner);
  }

  [Serializable]
  [EventAttribute(friendlyName = "Play Dialogue")]
  public class PlayDialogue : IScript {
    public Command<object> GetScript(EventRunner runner) {
      throw new SixteenFiftyException("not implemented");
    }
  }
}
