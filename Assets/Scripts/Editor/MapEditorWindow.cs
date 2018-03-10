using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow {
  [MenuItem("Window/Hex Map Editor")]
  public static void ShowWindow() {
    GetWindow<MapEditor>("Hex Map Editor");
  }
  
  void OnGUI() {
    var obj = Selection.activeGameObject;
    if(obj != null) {
      GUILayout.Label("hello " + obj.ToString(), EditorStyles.boldLabel);
    }
    else {
      GUILayout.Label("Nobody home!");
    }
  }
}
