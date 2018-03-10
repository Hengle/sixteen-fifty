using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner {
  private readonly EventScript e;
  public EventManager Manager {
    get;
    private set;
  }

  public EventRunner(EventManager manager, EventScript e) {
    this.e = e;
    Manager = manager;
  }

  public IEnumerator Coroutine {
    get {
      return e.GetScript(this).GetCoroutine();
    }
  }
}
