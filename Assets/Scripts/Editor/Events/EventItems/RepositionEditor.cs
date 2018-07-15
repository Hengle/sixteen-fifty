using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Variables;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Reposition))]
  public class RepositionEditor : ScriptedEventItemEditor {
    public override bool CanEdit(Type type) =>
      type == typeof(Reposition);

    public override void Draw(IScript _target) {
      var target = _target as Reposition;
      Debug.Assert(
        null != target,
        "RepositionEditor target is Reposition.");

      RecordChange("set position variable to reposition");
      target.position =
        EditorGUILayout.ObjectField(
          "Position",
          (UnityEngine.Object)target.position,
          typeof(IPositionVariable),
          false)
        as IPositionVariable;
    }
  }
}
