using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner {
  private readonly EventScript e;

  public EventManager Manager {
    get;
    private set;
  }

  public HexGrid Map {
    get;
    private set;
  }

  public EventRunner(EventManager manager, HexGrid map, EventScript e) {
    this.e = e;
    Map = map;
    Manager = manager;
  }

  public IEnumerator Coroutine {
    get {
      return e.GetScript(this).GetCoroutine();
    }
  }
}
