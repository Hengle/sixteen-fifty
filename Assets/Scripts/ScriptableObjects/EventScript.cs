using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScript {
  Commands.Command<object> GetScript(EventRunner runner);
}

public abstract class EventScript : ScriptableObject, IScript {
  public abstract Commands.Command<object> GetScript(EventRunner runner);
}
