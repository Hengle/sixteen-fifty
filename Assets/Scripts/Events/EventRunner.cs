using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using Behaviours;
  using EventItems;
  using TileMap;
  using UI;
  
  /**
  * \brief
  * A context in which event scripts are executed.
  */
  public class EventRunner {
    private readonly IScript e;

    public EventManager Manager {
      get;
      private set;
    }

    public HexGridManager GridManager {
      get;
      private set;
    }

    public event Action<IScript> EventComplete;

    public EventRunner(
      EventManager manager,
      HexGridManager gridManager,
      IScript e) {

      this.e = e;
      GridManager = gridManager;
      Manager = manager;
    }

    /**
     * \brief
     * Compiles the event into a coroutine.
     */
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

        EventComplete?.Invoke(e);
      }
    }
  }
}
