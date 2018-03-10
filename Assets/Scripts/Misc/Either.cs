using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Either<S, T> {
  public bool IsRight {
    get;
    private set;
  }

  private S left;
  private T right;

  public Either(S s) {
    IsRight = false;
    left = s;
  }

  public Either(T t) {
    IsRight = true;
    right = t;
  }

  public R Eliminate<R>(Func<S, R> ifLeft, Func<T, R> ifRight) {
    if(IsRight) {
      return ifRight(right);
    }
    else {
      return ifLeft(left);
    }
  }
}
