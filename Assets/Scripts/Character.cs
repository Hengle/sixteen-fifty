using System;

namespace SixteenFifty {
  /**
  * \brief Model class.
  *
  * Contains all a character's data.
  */
  [Serializable]
  public class Character {
    public string name;
    public Inventory inventory;
    public Equipment equipment;
  }
}
