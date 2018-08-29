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
    /**
     * \brief
     * Used by child controls to record changes for undo / automatic
     * saving.
     */
    new public static ScriptedEvent target;

    private static EqualityComparer<IScript> cmp = EqualityComparer<IScript>.Default;
    
    [SerializeField]
    EventItemControl control;

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

      // if(GUILayout.Button("Mark Changed")) {
      //   EditorUtility.SetDirty(target);
      // }

      Undo.RegisterCompleteObjectUndo(target, "modify scripted event");
      var root = target.root;
      if(control.Draw(ref root)) {
        Debug.Log("changed!");
        target.root = root;
        EditorUtility.SetDirty(target);
      }
    }
  }
}
