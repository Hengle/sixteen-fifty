using System;

namespace SixteenFifty.EventItems.Expressions {
  using Variables;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Variable")]
  public class ReadVariable<T> : IExpression<T> {
    public Variable<T> variable;
    public T Compute(EventRunner runner) => variable.Value;
  }
}
