using System;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Comparison")]
  public class Comparison : IExpression<bool> {
    public IComparisonOperator<int> operation;
    public IExpression<int> left;
    public IExpression<int> right;
    
    public bool Compute(EventRunner runner) {
      var l = left.Compute(runner);
      var r = right.Compute(runner);
      return operation.Compare(l, r);
    }
  }
}
