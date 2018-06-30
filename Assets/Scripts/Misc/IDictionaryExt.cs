using System;
using System.Collections.Generic;
using System.Linq;

public static class IDictionaryExt {
  /**
   * \brief
   * Transforms the values in a dictionary.
   *
   * The dictionary is not modified in-place; instead, an
   * IDictionary<K, W> is default-constructed and populated with the
   * results.
   */
  public static T Map<T, K, V, W>(this IDictionary<K, V> self, Func<V, W> f)
      where T : IDictionary<K, W>, new() {
    var d = new T();
    foreach(var kvp in self) {
      d[kvp.Key] = f(kvp.Value);
    }
    return d;
  }

  /**
   * \brief
   * Transforms the values in the `IDictionary<K, V>` in-place.
   */
  public static void Map<K, V>(this IDictionary<K, V> self, Func<V, V> f) {
    foreach(var k in self.Keys) {
      self[k] = f(self[k]);
    }
  }
}
