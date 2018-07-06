using System;
using System.Collections.Generic;

using UnityEngine;

using SixteenFifty.Serialization;

namespace SixteenFifty {
  using Commands;
  using EventItems;

  [CreateAssetMenu(menuName = "1650/Scripted Event")]
  public class ScriptedEvent : BasicScriptedEvent {
    new public string name;
    public IScript root;

    public override IScript Compile() => root;
  }

  // [Serializable]
  // [EventAttribute(friendlyName = "Play Dialogue")]
  // public class PlayDialogue : IScript {
  //   public Command<object> GetScript(EventRunner runner) {
  //     throw new SixteenFiftyException("not implemented");
  //   }
  // }
}
