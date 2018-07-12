using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExt {
  public static IEnumerable<T> NotNull<T>(this IEnumerable<T> self) where T : class =>
    self.Where(o => null != o);
}
