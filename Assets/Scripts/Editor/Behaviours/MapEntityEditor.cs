using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Behaviours;
  using TileMap;
  
  [CustomEditor(typeof(MapEntity))]
  public class MapEntityEditor : UnityEditor.Editor {
    new MapEntity target;

    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      target = base.target as MapEntity;
      Debug.Assert(
        target != null,
        "MapEntityEditor target is a MapEntity.");

      EditorGUILayout.LabelField(
        "Is moving?",
        target.IsMoving.ToString());

      EditorGUILayout.LabelField(
        "Current cell",
        target.CurrentCell?.coordinates.ToString() ?? "none");
    }
  }
}
