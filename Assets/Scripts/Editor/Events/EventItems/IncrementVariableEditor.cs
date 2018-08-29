using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Variables;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(IncrementVariable))]
  public class IncrementVariableEditor : ISubtypeEditor<IScript> {
    public IncrementVariableEditor(
      SubtypeSelectorContext<IScript> context) {
    }

    public bool CanEdit(Type type) =>
      type == typeof(IncrementVariable);

    public bool Draw(IScript _target) {
      var target = _target as IncrementVariable;
      Debug.Assert(
        null != target,
        "IncrementVariableEditor target is IncrementVariable.");

      var old = target.target;
      return
        old !=
        (target.target =
         EditorGUILayout.ObjectField(
           "Target",
           target.target,
           typeof(Variable<int>),
           false)
         as Variable<int>);
    }
  }
}
