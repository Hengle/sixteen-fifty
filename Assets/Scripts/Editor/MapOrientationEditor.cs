using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Behaviours;
  
  [CustomEditor(typeof(MapOrientation))]
  public class MapOrientationEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      var target = this.target as MapOrientation;
      Debug.Assert(
        null != target,
        "MapOrientationEditor's target is a MapOrientation.");

      EditorGUILayout.LabelField(
        "Orientation",
        target.Orientation.ToString());
    }
  }
}
