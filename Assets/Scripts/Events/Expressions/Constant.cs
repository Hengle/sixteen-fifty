using System;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Constant value")]
  public class Constant<T> : IExpression<T> {
    public T value;
    public T Compute(EventRunner runner) => value;
  }
}
