using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [Serializable]
  [ScriptedEventItemEditorFor(target = typeof(SimpleDialogue))]
  public class SimpleDialogueEditor : ScriptedEventItemEditor {
    new SimpleDialogue target;
    
    public bool CanEdit(Type type) =>
      type == typeof(SimpleDialogue);

    public void DrawInspector(IScript _target) {
      target = _target as SimpleDialogue;
      Debug.Assert(
        null != target,
        "Target of SimpleDialogueEditor is a SimpleDialogue.");
    }
  }
}
