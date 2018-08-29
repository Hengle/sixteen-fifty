using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems.Expressions;
  using Quests;

  [Serializable]
  [SubtypeEditorFor(target = typeof(HasQuest))]
  public class HasQuestEditor : ISubtypeEditor<IExpression<bool>> {
    [SerializeField]
    HasQuest target;

    public HasQuestEditor(SubtypeSelectorContext<IExpression<bool>> context) {
    }

    public bool CanEdit(Type type) => type == typeof(HasQuest);

    public bool Draw(IExpression<bool> _target) {
      target = _target as HasQuest;
      Debug.Assert(
        null != target,
        "HasQuestEditor target is HasQuest.");

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
