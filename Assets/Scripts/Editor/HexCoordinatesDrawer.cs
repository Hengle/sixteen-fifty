using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer {

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    var coordinates = new HexCoordinates(
      property.FindPropertyRelative("x").intValue,
      property.FindPropertyRelative("y").intValue,
      property.FindPropertyRelative("metrics").objectReferenceValue as HexMetrics);

    position = EditorGUI.PrefixLabel(position, label);
    GUI.Label(position, coordinates.ToString());
  }
}
