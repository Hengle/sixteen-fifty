using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using TileMap;
  
  [CustomEditor(typeof(HexMap))]
  public class HexMapInspector : UnityEditor.Editor {
    [SerializeField]
    new private HexMap target;

    public override void OnInspectorGUI() {
      target = base.target as HexMap;
      Debug.Assert(null != target, "HexMapEditor target is a HexMap");

      DrawDefaultInspector();
      DrawSizeWarnings();
      DrawClearTileChangedButton();

    }

    void DrawSizeWarnings() {
      if(target.width > 0) {
        EditorGUILayout.LabelField("Height", (target.tiles.Length / target.width).ToString());
        if(target.tiles.Length % target.width > 0) {
          GUILayout.Label("Warning: the map is non-rectangular!", EditorStyles.boldLabel);
        }
      }
      else {
        GUILayout.Label("Warning: the map has no width!", EditorStyles.boldLabel);
      }
    }

    void DrawClearTileChangedButton() {
      var b = GUILayout.Button(
        new GUIContent(
          "Clear TileChanged subscriptions",
          "The HexMap must not be loaded for this to be safe."));
      if(b) target.ClearTileChanged();
    }
  }
}
