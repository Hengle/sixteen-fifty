using System;

namespace SixteenFifty.EventItems.Expressions {
  using Variables;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Variable")]
  public class ReadVariable<T> : IExpression<T>, IEquatable<ReadVariable<T>> {
    public Variable<T> variable;
    public T Compute(EventRunner runner) => variable.Value;

    public bool Equals(ReadVariable<T> that) =>
      variable == that.variable;

    public bool Equals(IExpression<T> that) =>
      IEquatableExt.Equals(this, that);
  }
}
