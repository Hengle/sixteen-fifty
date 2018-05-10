using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Commands {
  /**
   * \brief
   * Completes when an event (firing an Action<T>) is raised.
   *
   * Because `event` members can't be passed to functions, this class
   * cheats in its constructor by taking a pair of Actions to
   * subscribe and unsubscribe from the event.
   *
   * \sa
   * AwaitActionExt
   */
  public class AwaitAction<T> : Command<T> {
    private Action<Action<T>> subscribe;
    private Action<Action<T>> unsubscribe;
    private T payload;
    private bool happened;

    /**
     * \brief
     * Constructs an Await action used the given pair of (un)subscribe functions.
     *
     * \param subscribe An action that subscribes the given Action<T>
     * to the event to await.
     * \param unsubscribe An action that unsubscribes the given
     * Action<T> from the event to await.
     */
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

  /**
   * \brief
   * Command extension methods to use AwaitAction conveniently.
   */
  public static class AwaitActionExt {
    /**
     * \brief
     * Chains the current Command with an AwaitAction constructed with
     * the given subscribe and unsubscribe actions.
     */
    public static Command<R> Await<T, R>(
      this Command<T> self,
      Action<Action<R>> subscribe,
      Action<Action<R>> unsubscribe) {

      return self.Then(_ => new AwaitAction<R>(subscribe, unsubscribe));
    }
  }
}
