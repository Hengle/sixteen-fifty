using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Interaction {
  /**
   * Gets the name of the interaction (shown in menus).
   */
  public string name; 

  /**
   * Gets the code that's executed when the interaction occurs.
   */
  public EventScript script;
}
