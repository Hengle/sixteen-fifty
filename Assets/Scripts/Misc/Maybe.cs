using System;
using System.Collections;
using System.Collections.Generic;

public class Maybe<T> : IEnumerable<T> {
  public static Maybe<R> Just<R>(R value) {
    return new Maybe<R>(value);
  }

  public static Maybe<R> Nothing<R>() {
    return new Maybe<R>();
  }

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

  public R Eliminate<R>(Func<R> ifNothing, Func<T, R> ifJust) {
    if(hasValue)
      return ifJust(value);
    else
      return ifNothing();
  }

  public Maybe<R> Then<R>(Func<T, Maybe<R>> continuation) {
    return Eliminate(() => Nothing<R>(), continuation);
  }

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
