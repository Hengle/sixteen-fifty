using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EventScript : ScriptableObject {
  public abstract Commands.Command<object> GetScript(EventRunner runner);
}
