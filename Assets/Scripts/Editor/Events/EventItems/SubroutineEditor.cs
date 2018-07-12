using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [SubtypeEditorFor(target = typeof(Subroutine))]
  public class SubroutineEditor : ScriptedEventItemEditor {
    Subroutine target;

    public SubroutineEditor(SubtypeSelectorContext<IScript> context) {
    }
    
    public override bool CanEdit(Type type) =>
      type == typeof(Subroutine);

    public override void Draw(IScript _target) {
      target = _target as Subroutine;
      Debug.Assert(
        null != target,
        "Target of SubroutineEditor is of type Subroutine.");
      target.target =
        EditorGUILayout.ObjectField(
          "Target",
          target.target,
          typeof(BasicScriptedEvent),
          false)
        as BasicScriptedEvent;
    }
  }
}
