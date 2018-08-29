using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Variables;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Reposition))]
  public class RepositionEditor : ISubtypeEditor<IScript> {
    public bool CanEdit(Type type) =>
      type == typeof(Reposition);

    public bool Draw(IScript _target) {
      var target = _target as Reposition;
      Debug.Assert(
        null != target,
        "RepositionEditor target is Reposition.");

      var old = target.position;
      return
        old !=
        (target.position =
         EditorGUILayout.ObjectField(
           "Position",
           (UnityEngine.Object)target.position,
           typeof(IPositionVariable),
           false)
         as IPositionVariable);
    }
  }
}
