using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using SixteenFifty.EventItems;
  using Reflection;

  [SubtypeEditorFor(target = typeof(ListScript))]
  public class ListScriptEditor : ScriptedEventItemEditor {
    [SerializeField]
    ListScript target;

    [SerializeField]
    List<SubtypeControl<IScript>> itemControls = new List<SubtypeControl<IScript>>();

    [SerializeField]
    List<FoldoutControl> foldouts = new List<FoldoutControl>();

    [SerializeField]
    SubtypeSelectorContext<IScript> context;

    public ListScriptEditor(SubtypeSelectorContext<IScript> context) {
      this.context = context;
    }

    public override bool CanEdit(Type type) {
      return type == typeof(ListScript);
    }

    public override void Draw(IScript _target) {
      target = _target as ListScript;
      Debug.Assert(
        null != target,
        "Target of ListScriptEditor is of type ListScript.");

      if(null == target.scripts)
        target.scripts = new List<IScript>();

      // updates the size of the target's scripts list
      DrawSizeControl();
      // matches the size of itemControls list to the target's scripts list
      UpdateItemControls();
      // draws the EventItemControls
      DrawElementControls();
    }

    /**
     * \brief
     * Ensures that the #itemControls and #foldouts lists matches the
     * size of #target's scripts list.
     */
    void UpdateItemControls() {
      itemControls.Resize(
        target.scripts.Count,
        _ => new EventItemControl("Event type", context));
      foldouts.Resize(
        target.scripts.Count,
        i =>
        new FoldoutControl(String.Format("Item {0}", i)));
    }

    void DrawSizeControl() {
      var oldSize = target.scripts.Count;
      var newSize = EditorGUILayout.DelayedIntField(
        "Size",
        oldSize);
      if(newSize != oldSize) {
        RecordChange("resize sequence of events");
        target.scripts.Resize(newSize);
      }
    }

    void DrawElementControls() {
      Debug.Assert(
        itemControls.Count == target.scripts.Count,
        "Selector count matches target scripts count.");
      Debug.Assert(
        foldouts.Count == target.scripts.Count,
        "Foldout count matches target scripts count.");

      for(var i = 0; i < itemControls.Count; i++) {
        var itemControl = itemControls[i];
        var subtarget = target.scripts[i];
        var foldout = foldouts[i];

        if(foldout.Draw()) {
          EditorGUI.indentLevel++;
          RecordChange("set target script in sequence");
          target.scripts[i] = itemControl.Draw(target.scripts[i]);
          EditorGUI.indentLevel--;
        }
      }
    }
  }
}
