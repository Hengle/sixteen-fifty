using System;
using System.Collections;
using System.Collections.Generic;

namespace SixteenFifty {
  public class Maybe<T> : IEnumerable<T> {
    public static Maybe<T> Just(T value) {
      return new Maybe<T>(value);
    }

    public static Maybe<T> Nothing() {
      return new Maybe<T>();
    }

    public static Maybe<R> FromNullable<R>(R value) where R : class =>
      null == value ? Maybe<R>.Nothing() : Maybe<R>.Just(value);

    private T value;
    private bool hasValue;

    public Maybe() {
      value = default(T);
      hasValue = false;
    }

    public Maybe(T value) {
      this.value = value;
      hasValue = true;
    }

    /**
     * \brief
     * Eliminates according to a function, or returns `null`.
     */
    public R Nullify<R>(Func<T, R> ifJust) where R : class =>
      Eliminate<R>(() => null, ifJust);

    /**
     * \brief
     * Eliminates by converting `Nothing` into `default(T)`.
     */
    public T Nullify() => Eliminate(() => default(T), x => x);

    /**
     * \brief
     * Eliminates into another type using a pair of functions.
     */
    public R Eliminate<R>(Func<R> ifNothing, Func<T, R> ifJust) {
      if(hasValue)
        return ifJust(value);
      else
        return ifNothing();
    }

    public Maybe<R> Then<R>(Func<T, Maybe<R>> continuation) {
      return Eliminate(() => Maybe<R>.Nothing(), continuation);
    }

    public Maybe<U> Map<U>(Func<T, U> f) =>
      Eliminate(
        () => Maybe<U>.Nothing(),
        t => Maybe<U>.Just(f(t)));

    public IEnumerator<T> GetEnumerator() {
      if(hasValue)
        yield return value;
      else
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      IEnumerator<T> e = GetEnumerator();
      return (IEnumerator)e;
    }
  }

  public static class IEnumerableMaybeExt {
    /**
     * \brief
     * Transforms and filters a sequence at one.
     *
     * Transforms the elements of the IEnumerable according to the
     * given function, and preserving the element in the output only
     * if it is Just.
     */
    public static IEnumerable<T> SelectWhere<S, T>(this IEnumerable<S> self, Func<S, Maybe<T>> f) {
      // have to assign default because our code is too complicated
      // for the definite-assignment checker
      T t = default(T);
      bool go = false;

      foreach(var s in self) {
        f(s).Eliminate(
          () => go = false,
          r => {
            t = r;
            return go = true;
          });
        if(go) yield return t;
      }
    }
  }
}
