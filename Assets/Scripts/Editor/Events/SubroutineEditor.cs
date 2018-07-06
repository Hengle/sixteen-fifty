using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [ScriptedEventItemEditorFor(target = typeof(Subroutine))]
  public class SubroutineEditor : ScriptedEventItemEditor {
    new Subroutine target;
    
    public bool CanEdit(Type type) =>
      type == typeof(Subroutine);

    public void DrawInspector(IScript _target) {
      target = _target as Subroutine;
      Debug.Assert(
        null != target,
        "Target of SubroutineEditor is of type Subroutine.");
      target.target =
        EditorGUILayout.ObjectField(
          "Target",
          target.target,
          typeof(ScriptedEvent),
          false)
        as ScriptedEvent;
    }
  }
}
