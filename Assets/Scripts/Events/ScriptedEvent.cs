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
}
