using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Commands;

/**
 * \brief
 * Applied to classes that derive from BasicScript and can be used in
 * the event system.
 */
[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : Attribute {
  public string friendlyName;
}

public interface IScript {
  Command<object> GetScript(EventRunner runner);
}

// [Serializable]
// public abstract class BasicScript {
//   public abstract Command<object> GetScript(EventRunner runner);
// }

[Serializable]
public abstract class EventScript : ScriptableObject, IScript {
  public abstract Command<object> GetScript(EventRunner runner);
}
