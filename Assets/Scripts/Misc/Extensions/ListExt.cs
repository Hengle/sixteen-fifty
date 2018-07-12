using System;
using System.Collections.Generic;

namespace SixteenFifty {
  public static class ListExt {
    /**
    * \brief
    * Resizes the list to the given count.
    *
    * If new elements must be added, they are set to `default(T)`.
    */
    public static void Resize<T>(this List<T> self, int size) {
      self.Resize(size, _ => default(T));
    }

    /**
    * \brief
    * Resize the list to the given count.
    *
    * If new elements must be added, they are constructed using the
    * given `create` function, to which is passed the index of the
    * element to create.
    */
    public static void Resize<T>(
        this List<T> self, int size, Func<int, T> create) {
      var d = size - self.Count;
      while(d --> 0) self.Add(create(self.Count));

      d = self.Count - size;
      if(d > 0) self.RemoveRange(self.Count - 1 - d, d);
    }
  }
}
