using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Constant value")]
  public class Constant<T> : IExpression<T>, IEquatable<Constant<T>> {
    public T value;
    public T Compute(EventRunner runner) => value;

    public bool Equals(Constant<T> that) =>
      EqualityComparer<T>.Default.Equals(value, that.value);

    public bool Equals(IExpression<T> _that) {
      var that = _that as Constant<T>;
      return null != that && Equals(that);
    }
  }
}
