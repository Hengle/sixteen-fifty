using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Reflection;

  [CustomEditor(typeof(ScriptedEvent))]
  public class ScriptedEventEditor : UnityEditor.Editor {
    [SerializeField]
    EventItemControl control;

    new ScriptedEvent target;

    SubtypeSelectorContext<IScript> context;
    
    void OnEnable() {
      if(context == null)
        context = new SubtypeSelectorContext<IScript>();

      if(control == null)
        control = new EventItemControl("Root event", context);
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

      if(GUILayout.Button("Mark Changed")) {
        EditorUtility.SetDirty(target);
      }

      target.root = control.Draw(target.root);
    }
  }
}
