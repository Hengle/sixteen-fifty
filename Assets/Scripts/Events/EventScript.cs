using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EventScript {
  /**
   * Gets the code of the event, which can use the environment of the
   * current event runner.
   */
  Commands.Command<object> GetScript(EventRunner runner);
}
