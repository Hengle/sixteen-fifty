using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Quests;

  [Serializable]
  [SubtypeEditorFor(target = typeof(GiveQuest))]
  public class GiveQuestEditor : ScriptedEventItemEditor {
    [SerializeField]
    GiveQuest target;

    public GiveQuestEditor(SubtypeSelectorContext<IScript> context) {
    }

    public override bool CanEdit(Type type) =>
      type == typeof(GiveQuest);

    public override void Draw(IScript _target) {
      target = _target as GiveQuest;
      Debug.Assert(
        null != target,
        "GiveQuestEditor target is GiveQuest.");

      target.questLog =
        EditorGUILayout.ObjectField(
          "Quest Log",
          target.questLog,
          typeof(QuestLog),
          false)
        as QuestLog;

      target.quest =
        EditorGUILayout.ObjectField(
          "Quest",
          target.quest,
          typeof(Quest),
          false)
        as Quest;
    }
  }
}
