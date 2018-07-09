using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using TileMap;

  /**
   * \brief
   * A `PropertyDrawer` for HexCoordinates objects with the
   * ReadonlyHexCoordinatesAttribute attribute.
   *
   * The coordinates are simply drawn as a label that looks like
   * `(x, y, z)`.
   */
  [CustomPropertyDrawer(typeof(ReadonlyHexCoordinatesAttribute))]
  public class ReadonlyHexCoordinatesDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      var coordinates = new HexCoordinates(
        property.FindPropertyRelative("x").intValue,
        property.FindPropertyRelative("y").intValue,
        property.FindPropertyRelative("metrics").objectReferenceValue as HexMetrics);

      position = EditorGUI.PrefixLabel(position, label);
      GUI.Label(position, coordinates.ToString());
    }
  }
  
  /**
   * \brief
   * A `PropertyDrawer` for HexCoordinates that supports editing.
   *
   * The property lets the developer edit the `x` and `y` components
   * of the cubical HexCoordinates representation, while displaying
   * the full `(x, y, z)` triple underneath.
   */
  [CustomPropertyDrawer(typeof(HexCoordinates))]
  public class HexCoordinatesDrawer : PropertyDrawer {
    float lastHeight = 0f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      position = EditorGUI.PrefixLabel(position, label);

      var initialY = position.y;
      var lineHeight = base.GetPropertyHeight(property, label);
      var lmgr = new LineRectManager(position, lineHeight);

      Action<string> prop = s =>
        EditorGUI.PropertyField(
          lmgr.NextLine().WithHeight(lineHeight),
          property.FindPropertyRelative(s),
          new GUIContent(s.ToUpper()));

      prop("x");
      prop("y");

      var coordinates = new HexCoordinates(
        property.FindPropertyRelative("x").intValue,
        property.FindPropertyRelative("y").intValue,
        property.FindPropertyRelative("metrics").objectReferenceValue as HexMetrics);

      GUI.Label(
        lmgr.NextLine().WithHeight(lineHeight),
        coordinates.ToString());

      lastHeight = lmgr.Position.y - initialY;
    }

    public override float GetPropertyHeight(
      SerializedProperty property,
      GUIContent label) =>
      lastHeight;
  }
}
