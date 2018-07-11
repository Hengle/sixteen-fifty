using System;

namespace SixteenFifty.EventItems {
  using Quests;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Give Quest")]
  public class GiveQuest : ImmediateScript {
    public QuestLog questLog;
    public Quest quest;

    public override void Call(EventRunner runner) {
      questLog.AddQuest(quest);
    }
  }
}
