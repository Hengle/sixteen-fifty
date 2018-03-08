using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class for manipulating paths abstractly.
 */
public static class Path {
  /**
   * Constructs a path starting at the given initial node, following the breadcrumbs.
   * Each element of the path is yielded sequentially.
   * This includes both boundary nodes.
   * Breadcrumbs associate to each node who that node's parent is.
   * In essence, this is implicitly a rooted tree structure.
   */
  public static IEnumerator<T> Construct<T>(T initial, Dictionary<T, T> breadcrumbs) {
    yield return initial;
    for(var current = initial; breadcrumbs[current] != null; current = breadcrumbs[current]) {
      yield return breadcrumbs[current];
    }
  }
}
