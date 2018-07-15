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

      RecordChange("Set quest log");
      target.questLog =
        EditorGUILayout.ObjectField(
          "Quest Log",
          target.questLog,
          typeof(QuestLog),
          false)
        as QuestLog;

      RecordChange("Set quest");
      var oldQuest = target.quest;
      target.quest =
        EditorGUILayout.ObjectField(
          "Quest",
          oldQuest,
          typeof(Quest),
          false)
        as Quest;
      if(oldQuest != target.quest) {
        Debug.Log("change happened");
        ChangeHappened();
      }
    }
  }
}
