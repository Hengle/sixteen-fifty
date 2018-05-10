using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An interaction is essentially just a named EventScript.
 * Typically, menus are populated with buttons created from a list of interactions.
 */
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
