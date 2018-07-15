using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;

  [Serializable]
  [SubtypeEditorFor(target = typeof(DebugMessage))]
  public class DebugMessageEditor : ScriptedEventItemEditor {
    [SerializeField]
    DebugMessage target;

    public DebugMessageEditor(SubtypeSelectorContext<IScript> context) {
    }

    public override bool CanEdit(Type type)  => type == typeof(DebugMessage);

    public override void Draw(IScript _target) {
      target = _target as DebugMessage;
      Debug.Assert(
        null != target,
        "Target of DebugMessageEditor is of type DebugMessage.");

      RecordChange("Set debug message");
      target.message = EditorGUILayout.DelayedTextField(
        "message",
        target.message);
    }
  }
}
