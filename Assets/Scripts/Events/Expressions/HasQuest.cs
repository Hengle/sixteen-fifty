using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems.Expressions {
  using Quests;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Has Quest?")]
  public class HasQuest : IExpression<bool> {
    public QuestLog questLog;
    public Quest quest;

    public bool Compute(EventRunner runner) => questLog.HasQuest(quest);

    public bool Equals(HasQuest that) =>
      questLog == that.questLog &&
      quest == that.quest;

    public bool Equals(IExpression<bool> _that) {
      var that = _that as HasQuest;
      return null != that && Equals(that);
    }
  }
}
