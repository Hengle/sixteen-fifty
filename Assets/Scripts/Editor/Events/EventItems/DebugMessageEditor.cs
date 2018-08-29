using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;

  [Serializable]
  [SubtypeEditorFor(target = typeof(DebugMessage))]
  public class DebugMessageEditor : ISubtypeEditor<IScript> {
    [SerializeField]
    DebugMessage target;

    public DebugMessageEditor(SubtypeSelectorContext<IScript> context) {
    }

    public bool CanEdit(Type type) =>
      type == typeof(DebugMessage);

    public bool Draw(IScript _target) {
      target = _target as DebugMessage;
      Debug.Assert(
        null != target,
        "Target of DebugMessageEditor is of type DebugMessage.");

      // return whether the new message is distinct from the old one
      var old = target.message;
      return
        old !=
        (target.message = EditorGUILayout.DelayedTextField(
          "message",
          target.message));
    }
  }
}
