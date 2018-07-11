using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Quests;
  
  [CustomEditor(typeof(QuestLog))]
  public class QuestLogEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var target = this.target as QuestLog;
      Debug.Assert(
        null != target,
        "QuestLogEditor target is QuestLog.");

      GUI.tooltip =
        "Resets and removes each quest in the log.";
      var b = GUILayout.Button("Reset quest log");
      GUI.tooltip = null;
      if(b) {
        foreach(var quest in target.quests) {
          quest.progress.value = 0;
        }
        target.quests.Clear();
      }

      DrawDefaultInspector();
    }
  }
}
