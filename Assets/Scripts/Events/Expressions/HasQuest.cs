using System;

namespace SixteenFifty.EventItems.Expressions {
  using Quests;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Has Quest?")]
  public class HasQuest : IExpression<bool> {
    public QuestLog questLog;
    public Quest quest;

    public bool Compute(EventRunner runner) => questLog.HasQuest(quest);
  }
}
