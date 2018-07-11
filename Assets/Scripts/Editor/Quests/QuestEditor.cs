using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Quests;
  [CustomEditor(typeof(Quest))]
  public class QuestEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var target = this.target as Quest;
      Debug.Assert(
        null != target,
        "QuestEditor target is Quest.");

      if(GUILayout.Button("Reset quest")) {
        Undo.RecordObject(target.progress, "Reset quest progress");
        target.progress.value = 0;
      }

      DrawDefaultInspector();
    }
  }
}
