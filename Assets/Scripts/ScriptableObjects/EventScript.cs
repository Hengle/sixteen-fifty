using System;
using UnityEngine;

using Commands;

public interface IScript {
  Command<object> GetScript(EventRunner runner);
}

[Serializable]
public abstract class EventScript : ScriptableObject, IScript {
  public abstract Command<object> GetScript(EventRunner runner);
}
