using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using TileMap;

  [Serializable]
  [SubtypeEditorFor(target = typeof(JumpToMap))]
  public class JumpToMapEditor : ISubtypeEditor<IScript> {
    public JumpToMapEditor(SubtypeSelectorContext<IScript> context) {
    }
    
    public bool CanEdit(Type type) =>
      type == typeof(JumpToMap);

    public bool Draw(IScript _target) {
      var target = _target as JumpToMap;
      Debug.Assert(
        null != target,
        "JumpToMapEditor target is JumpToMap.");

      var old = target.map;
      return
        old !=
        (target.map =
         EditorGUILayout.ObjectField(
           "Destination",
           target.map,
           typeof(BasicMap),
           false)
         as BasicMap);
    }
  }
}
