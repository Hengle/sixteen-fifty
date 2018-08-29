using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Quests;

  [Serializable]
  [SubtypeEditorFor(target = typeof(GiveQuest))]
  public class GiveQuestEditor : ISubtypeEditor<IScript> {
    [SerializeField]
    GiveQuest target;

    public GiveQuestEditor(SubtypeSelectorContext<IScript> context) {
    }

    public bool CanEdit(Type type) =>
      type == typeof(GiveQuest);

    public bool Draw(IScript _target) {
      target = _target as GiveQuest;
      Debug.Assert(
        null != target,
        "GiveQuestEditor target is GiveQuest.");

      var b = false;

      var old1 = target.questLog;
      target.questLog =
        EditorGUILayout.ObjectField(
          "Quest Log",
          old1,
          typeof(QuestLog),
          false)
        as QuestLog;
      b = b || old1 != target.questLog;

      var old2 = target.quest;
      target.quest =
        EditorGUILayout.ObjectField(
          "Quest",
          old2,
          typeof(Quest),
          false)
        as Quest;
      b = b || old2 != target.quest;

      return b;
    }
  }
}
