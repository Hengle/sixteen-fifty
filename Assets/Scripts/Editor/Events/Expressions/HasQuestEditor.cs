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

    public void Draw(IExpression<bool> _target) {
      target = _target as HasQuest;
      Debug.Assert(
        null != target,
        "HasQuestEditor target is HasQuest.");

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
