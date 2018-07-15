using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;

  [SubtypeEditorFor(target = typeof(FadeScreen))]
  public class FadeScreenEditor : ScriptedEventItemEditor {
    public FadeScreenEditor(SubtypeSelectorContext<IScript> context) {
    }
    
    public override bool CanEdit(Type type) =>
      type == typeof(FadeScreen);

    public override void Draw(IScript _target) {
      var target = _target as FadeScreen;
      Debug.Assert(
        _target != null,
        "FadeScreenEditor target is FadeScreen.");

      RecordChange("set fade screen direction");
      target.direction =
        (FadeDirection)
        EditorGUILayout.EnumPopup(
          "Direction",
          target.direction);
    }
  }
}
