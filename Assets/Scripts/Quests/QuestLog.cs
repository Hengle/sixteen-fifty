using System;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.Quests {
  [CreateAssetMenu(menuName = "1650/Quests/Quest Log")]
  public class QuestLog : ScriptableObject {
    public List<Quest> quests;

    public event Action<Quest> QuestAdded;

    public bool HasQuest(Quest quest) => quests.Contains(quest);

    /**
     * \brief
     * Adds the given quest if it isn't already present, and raises
     * QuestAdded.
     */
    public void AddQuest(Quest quest) {
      if(HasQuest(quest)) {
        Debug.LogWarningFormat(
          "Tried to add quest {0} twice.",
          quest.name);
        return;
      }

      quests.Add(quest);
      QuestAdded?.Invoke(quest);
    }
  }
}
