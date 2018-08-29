using System;

namespace SixteenFifty.EventItems {
  using Quests;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Give Quest")]
  public class GiveQuest : ImmediateScript, IEquatable<GiveQuest> {
    public QuestLog questLog;
    public Quest quest;

    public override void Call(EventRunner runner) {
      questLog.AddQuest(quest);
    }

    public bool Equals(GiveQuest that) =>
      questLog == that.questLog &&
      quest == that.quest;

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
