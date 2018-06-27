using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  [CustomEditor(typeof(ListScript))]
  public class BasicEventPropertyDrawer : UnityEditor.Editor {
    EventSelectorEditor rootEventSelector;

    void Awake() {
      rootEventSelector = new EventSelectorEditor();
    }

    public override void OnInspectorGUI() {
      serializedObject.Update();


      var scripts = serializedObject.FindProperty("scripts");

      scripts.arraySize = EditorGUILayout.IntField("Size", scripts.arraySize);

      for(int i = 0; i < scripts.arraySize; i++) {
        var script = scripts.GetArrayElementAtIndex(i);
        EditorGUILayout.LabelField("Element " + i.ToString());
        EditorGUI.indentLevel += 1;
        rootEventSelector.rootEvent = script.objectReferenceValue as EventScript;
        rootEventSelector.Show(target);
        script.objectReferenceValue = rootEventSelector.rootEvent;
        EditorGUI.indentLevel -= 1;
      }

      serializedObject.ApplyModifiedProperties();
    }
  }

  public class BasicScriptEditor : UnityEditor.Editor {
    EventSelectorEditor rootEventSelector;

    void Awake() {
      rootEventSelector = new EventSelectorEditor();
    }

    public override void OnInspectorGUI() {
      serializedObject.Update();
      var script = serializedObject.FindProperty("script");
      rootEventSelector.rootEvent = script.objectReferenceValue as EventScript;
      rootEventSelector.Show(target);
      script.objectReferenceValue = rootEventSelector.rootEvent;

      serializedObject.ApplyModifiedProperties();
    }
  }
}
