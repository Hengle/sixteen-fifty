using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class IDictionaryExt {
  public static void DebugLog<K, V>(this IDictionary<K, V> self) =>
    Debug.Log(
      String.Join(
        "\n",
        self.Select(
          p =>
          String.Format(
            "{0} -> {1}",
            p.Key, p.Value))
        .ToArray()));
        
  /**
   * \brief
   * Forms a list `L` such that for all `i`, `L[i] = D[i]` where `D`
   * is the dictionary.
   *
   * This method specifically constructs a `List<T>`.
   */
  public static List<T> ToIndexList<T>(this IDictionary<int, T> self) =>
    self.ToIndexList<T, List<T>>();

  /**
   * \brief
   * Forms a list `L` such that for all `i`, `L[i] = D[i]` where `D`
   * is the dictionary.
   *
   * This method can construct any IList of your choosing to hold the results.
   */
  public static R ToIndexList<T, R>(
    this IDictionary<int, T> self) where R : IList<T>, new() {

    var l = self.ToList();
    l.Sort(
      (x, y) =>
      Comparer<int>.Default.Compare(x.Key, y.Key));

    var result = new R();
    if(l.Count == 0)
      return result;
    if(l[0].Key < 0)
      throw new SixteenFiftyException(
        "Cannot form list from index-dictionary with negative keys.");

    var lastIndex = -1;
    foreach(var p in l) {
      // pad the list with defaults until we get to the next index
      for(var j = p.Key; j < lastIndex + 1; j++) {
        result.Add(default(T));
      }
      result.Add(p.Value);
      lastIndex = p.Key;
    }

    return result;
  }

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
