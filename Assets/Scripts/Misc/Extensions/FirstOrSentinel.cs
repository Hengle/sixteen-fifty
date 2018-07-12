using System.Collections.Generic;

public static class FirstOrSentinelExt {
  public static T FirstOrSentinel<T>(this IEnumerable<T> source, T sentinel) {
    foreach(var t in source) {
      return t;
    }
    return sentinel;
  }
}
