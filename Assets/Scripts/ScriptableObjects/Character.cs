using System;

using UnityEngine;

namespace SixteenFifty {
  using Quests;

  /**
  * \brief Model class for the player character.
  *
  * Contains all a character's data.
  */
  [CreateAssetMenu(menuName = "1650/Character")]
  public class Character : ScriptableObject {
    new public string name;
    public Inventory inventory;
    public QuestLog questLog;
    public Equipment equipment;
  }
}
