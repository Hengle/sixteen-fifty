using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexMap))]
public class HexMapInspector : Editor {
  public override void OnInspectorGUI() {
    var map = target as HexMap;
    base.OnInspectorGUI();

    if(map.width > 0) {
      EditorGUILayout.LabelField("Height", (map.tiles.Length / map.width).ToString());
      if(map.tiles.Length % map.width > 0) {
        GUILayout.Label("Warning: the map is non-rectangular!", EditorStyles.boldLabel);
      }
    }
    else {
      GUILayout.Label("Warning: the map has no width!", EditorStyles.boldLabel);
    }
  }
}
