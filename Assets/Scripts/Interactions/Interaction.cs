using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : ScriptableObject {
  /**
   * Gets the name of the interaction (shown in menus).
   */
  public abstract string Name { get; }

  /**
   * Gets the code that's executed when the interaction occurs.
   */
  public EventScript script;
}
