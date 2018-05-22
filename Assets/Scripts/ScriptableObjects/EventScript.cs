using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Commands;

public interface IScript {
  Command<object> GetScript(EventRunner runner);
}

[Serializable]
public abstract class BasicScript {
  public abstract Command<object> GetScript(EventRunner runner);
}

[Serializable]
public abstract class EventScript : ScriptableObject, IScript {
  public abstract Command<object> GetScript(EventRunner runner);
}
