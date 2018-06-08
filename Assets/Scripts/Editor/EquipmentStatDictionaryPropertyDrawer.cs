using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

// public static class ListDictionaryPropertyDrawer<K, V> {
//   public static void DrawListDictionary(Rect position, ListDictionary<K, V> dict) {
//     for(var i = 0; i < dict.Count; i++) {
//       var p = dict[i];
//       position = EditorGUI.PrefixLabel(position, "Element " + i.ToString())
// 
//       EditorGUI.BeginHorizontal();
// 
//       EditorGUI.EndHorizontal();
//     }
//   }
// }

[CustomPropertyDrawer(typeof(EquipmentStatDictionary))]
public class EquipmentStatDictionaryPropertyDrawer : PropertyDrawer {
  EquipmentStat nextStat = null;
  StatValue nextStatValue = StatValue.Uninitialized;

  bool[] foldoutStates = null;
  List<int> toDelete = new List<int>();


  float lastHeight = 0f;

  public override void OnGUI(
    Rect position, SerializedProperty property, GUIContent label) {

    float initialY = position.y;

    var lineHeight = base.GetPropertyHeight(property, label);
    var lmgr = new LineRectManager(position, lineHeight);

    var keysProp = property.FindPropertyRelative("keys");
    var valuesProp = property.FindPropertyRelative("values");
    Debug.Assert(null != keysProp, "the keys property exists");
    Debug.Assert(null != valuesProp, "the values property exists");
    Debug.Assert(
      keysProp.isArray && valuesProp.isArray,
      "the keys and values are arrays");
    Debug.Assert(
      keysProp.arraySize == valuesProp.arraySize,
      "the keys and values have the same length");

    toDelete.Reverse();
    foreach(var i in toDelete) {
      keysProp.DeleteArrayElementAtIndex(i);
    }
    toDelete.Clear();
    
    var n = foldoutStates?.Length ?? 0;
    if(n != keysProp.arraySize)
      Array.Resize(ref foldoutStates, keysProp.arraySize);

    for(var i = 0; i < keysProp.arraySize; i++) {
      // position for the foldout and the delete button
      var ps = lmgr.NextLine.WithHeight(lineHeight).SplitX(2);

      var niceName = keysProp.GetArrayElementAtIndex(i).displayName;

      if(foldoutStates[i] =
         EditorGUI.Foldout(
           ps[0],
           foldoutStates[i],
           niceName)) {

        EditorGUI.indentLevel += 1;

        var keyProp = keysProp.GetArrayElementAtIndex(i);
        EditorGUI.PropertyField(
          lmgr.NextBy(EditorGUI.GetPropertyHeight(keyProp, null, true)),
          keyProp,
          new GUIContent("key"),
          true);

        var valueProp = valuesProp.GetArrayElementAtIndex(i);
        EditorGUI.PropertyField(
          lmgr.NextBy(EditorGUI.GetPropertyHeight(valueProp, null, true)),
          valueProp,
          new GUIContent("value"),
          true);

        EditorGUI.indentLevel -= 1;
      }

      var p = ps[1];
      var w = 25;
      var x = p.x + p.width - w;
      p.x = x;
      p.width = w;
      if(GUI.Button(p, "X")) {
        toDelete.Add(i);
      }
    }

    var countProp = property.FindPropertyRelative("count");
    Debug.Assert(null != countProp, "the count property exists");

    EditorGUI.LabelField(lmgr.NextLine, "Add another!");

    DrawNextKeyEditor(lmgr);
    DrawNextValueEditor(lmgr);

    if(GUI.Button(lmgr.NextLine.WithHeight(lineHeight), "Add")) {
      Debug.Log("button clicked!");
      AddNewKeyValuePair(countProp, keysProp, valuesProp);
    }

    lastHeight = lmgr.Position.y + lineHeight - initialY;
  }

  private void AddNewKeyValuePair(
    SerializedProperty countProp,
    SerializedProperty keysProp,
    SerializedProperty valuesProp) {

    countProp.intValue++;
    keysProp.arraySize++;
    keysProp
      .GetArrayElementAtIndex(keysProp.arraySize - 1)
      .objectReferenceValue = nextStat;
    valuesProp.arraySize++;
    var foo = valuesProp.GetArrayElementAtIndex(valuesProp.arraySize - 1);
    foo.FindPropertyRelative("value").floatValue =
      nextStatValue.value;
    foo.FindPropertyRelative("statFormatter")
      .objectReferenceValue = nextStatValue.statFormatter;
  }

  private void DrawNextKeyEditor(LineRectManager lmgr) {
    nextStat =
      EditorGUI.ObjectField(
        lmgr.NextLine.WithHeight(lmgr.LineHeight),
        "key",
        nextStat,
        typeof(EquipmentStat))
      as EquipmentStat;
  }

  private void DrawNextValueEditor(LineRectManager lmgr) {
    SixteenFiftyGUI.StatValueField(lmgr, nextStatValue);
  }

  public override float GetPropertyHeight(
    SerializedProperty property,
    GUIContent label) {

    return lastHeight;
  }
}
