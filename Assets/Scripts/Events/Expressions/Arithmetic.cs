using System;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Arithmetic")]
  public class Arithmetic : IExpression<int> {
    public IArithmeticOperation operation;
    public IExpression<int> left;
    public IExpression<int> right;

    public int Compute(EventRunner runner) =>
      operation.Compute(left.Compute(runner), right.Compute(runner));
  }
}
