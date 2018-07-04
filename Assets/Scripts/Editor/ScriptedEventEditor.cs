using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Reflection;

  [CustomEditor(typeof(ScriptedEvent))]
  public class ScriptedEventEditor : UnityEditor.Editor {
    [SerializeField]
    EventItemControl control;

    new ScriptedEvent target;

    ScriptedEventItemEditor editor;
    
    void OnEnable() {
      if(control == null)
        control = new EventItemControl("Root event");
    }

    void OnDisable() {
    }

    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      // types ftw
      target = base.target as ScriptedEvent;
      Debug.Assert(
        null != target,
        "ScriptedEventEditor target is a ScriptedEvent.");

      target.root = control.Draw(target.root);
    }
  }
}
