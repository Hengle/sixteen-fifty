using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A context in which scripts are executed.
 */
public class EventRunner {
  private readonly IScript e;

  public EventManager Manager {
    get;
    private set;
  }

  public HexGrid Map {
    get;
    private set;
  }

  public event Action<IScript> EventComplete;

  public EventRunner(EventManager manager, HexGrid map, IScript e) {
    this.e = e;
    Map = map;
    Manager = manager;
  }

  public IEnumerator Coroutine {
    get {
      Debug.Assert(null != e, "Event script is not null");
      var s = e.GetScript(this);
      Debug.Assert(null != s, "Command is not null");
      var coro = s.GetCoroutine();
      Debug.Assert(null != coro, "Coroutine of command is not null");
      foreach(var o in new TrivialEnumerable(coro)) {
        yield return o;
      }
      if(null != EventComplete) {
        EventComplete(e);
      }
    }
  }
}
