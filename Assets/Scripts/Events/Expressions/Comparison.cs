using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Comparison")]
  public class Comparison : IExpression<bool>, IEquatable<Comparison> {
    public IComparisonOperator<int> operation;
    public IExpression<int> left;
    public IExpression<int> right;
    
    public bool Compute(EventRunner runner) {
      var l = left.Compute(runner);
      var r = right.Compute(runner);
      return operation.Compare(l, r);
    }

    public bool Equals(Comparison that) {
      var cmp1 = EqualityComparer<IComparisonOperator<int>>.Default;
      var cmp2 = EqualityComparer<IExpression<int>>.Default;
      return
        cmp1.Equals(operation, that.operation) &&
        cmp2.Equals(left, that.left) &&
        cmp2.Equals(right, that.right);
    }

    public bool Equals(IExpression<bool> _that) {
      var that = _that as Comparison;
      return null != that && Equals(that);
    }
  }
}
