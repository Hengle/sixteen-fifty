using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using TileMap;

  [Serializable]
  [SubtypeEditorFor(target = typeof(JumpToMap))]
  public class JumpToMapEditor : ScriptedEventItemEditor {
    public JumpToMapEditor(SubtypeSelectorContext<IScript> context) {
    }
    
    public override bool CanEdit(Type type) =>
      type == typeof(JumpToMap);

    public override void Draw(IScript _target) {
      var target = _target as JumpToMap;
      Debug.Assert(
        null != target,
        "JumpToMapEditor target is JumpToMap.");

      target.map =
        EditorGUILayout.ObjectField(
          "Destination",
          target.map,
          typeof(BasicMap),
          false)
        as BasicMap;
    }
  }
}
