using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty {
  /**
  * An interaction is essentially just a named EventScript.
  * Typically, menus are populated with buttons created from a list of interactions.
  */
  [Serializable]
  public class Interaction {
    /**
     * \brief
     * The name of the interaction (shown in menus).
     */
    public string name; 

    /**
     * \brief
     * The code that's executed when the interaction occurs.
     */
    public BasicScriptedEvent script;
  }
}
