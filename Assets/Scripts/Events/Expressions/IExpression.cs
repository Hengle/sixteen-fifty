using System;

namespace SixteenFifty.EventItems.Expressions {
  using Variables;
  
  /**
   * \brief
   * Represents a computation that produces a result of type `T`.
   *
   * This is used by the scripted events system.
   */
  public interface IExpression<T> {
    T Compute(EventRunner runner);
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "Constant value")]
  public class Constant<T> : IExpression<T> {
    public T value;
    public T Compute(EventRunner runner) => value;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "Variable")]
  public class ReadVariable<T> : IExpression<T> {
    public Variable<T> variable;
    public T Compute(EventRunner runner) => variable.Value;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "Comparison")]
  public class Comparison : IExpression<bool> {
    public IExpression<int> left;
    public IExpression<int> right;
    public IComparisonOperator<int> op;
    
    public bool Compute(EventRunner runner) {
      var l = left.Compute(runner);
      var r = right.Compute(runner);
      return op.Compare(l, r);
    }
  }
}
