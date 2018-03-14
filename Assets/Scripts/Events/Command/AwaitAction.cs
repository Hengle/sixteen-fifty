using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Commands {
  /**
   * A command that completes when an event (firing an Action<T>) is raised.
   */
  public class AwaitAction<T> : Command<T> {
    private Action<Action<T>> subscribe;
    private Action<Action<T>> unsubscribe;
    private T payload;
    private bool happened;

    public AwaitAction(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) {
      this.subscribe = subscribe;
      this.unsubscribe = unsubscribe;
      happened = false;
    }

    private void OnAction(T payload) {
      this.payload = payload;
      happened = true;
    }
  
    public override IEnumerator GetCoroutine() {
      subscribe(OnAction);
      while(!happened) yield return null;
      unsubscribe(OnAction);
      result = payload;
      yield break;
    }
  }

  public static class AwaitActionExt {
    public static Command<R> Await<T, R>(
      this Command<T> self,
      Action<Action<R>> subscribe,
      Action<Action<R>> unsubscribe) {

      return self.Then(_ => new AwaitAction<R>(subscribe, unsubscribe));
    }
  }
}
