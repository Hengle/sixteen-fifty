using System;

namespace SixteenFifty.EventItems.Expressions {
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
