using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;

  [SubtypeEditorFor(target = typeof(FadeScreen))]
  public class FadeScreenEditor : ISubtypeEditor<IScript> {
    public FadeScreenEditor(SubtypeSelectorContext<IScript> context) {
    }
    
    public bool CanEdit(Type type) =>
      type == typeof(FadeScreen);

    public bool Draw(IScript _target) {
      var target = _target as FadeScreen;
      Debug.Assert(
        _target != null,
        "FadeScreenEditor target is FadeScreen.");

      var old = target.direction;
      return
        old !=
        (target.direction =
         (FadeDirection)
         EditorGUILayout.EnumPopup(
           "Direction",
           target.direction));
    }
  }
}
