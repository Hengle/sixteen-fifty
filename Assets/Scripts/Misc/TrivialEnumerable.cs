using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrivialEnumerable<T> : IEnumerable<T> {
  private IEnumerator<T> enumerator;

  public TrivialEnumerable(IEnumerator<T> enumerator) {
    this.enumerator = enumerator;
  }

  public IEnumerator<T> GetEnumerator() => enumerator;

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
