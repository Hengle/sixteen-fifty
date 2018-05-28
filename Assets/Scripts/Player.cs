using System;

/**
 * \brief Model class.
 *
 * Contains all a player character's data.
 */
[Serializable]
public class Player {
  public string name;
  public Inventory inventory;
  public Equipment equipment;
}
