using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Arithmetic")]
  public class Arithmetic : IExpression<int>, IEquatable<Arithmetic> {
    public IArithmeticOperation operation;
    public IExpression<int> left;
    public IExpression<int> right;

    public int Compute(EventRunner runner) =>
      operation.Compute(left.Compute(runner), right.Compute(runner));

    public bool Equals(Arithmetic that) {
      var cmp1 = EqualityComparer<IArithmeticOperation>.Default;
      var cmp2 = EqualityComparer<IExpression<int>>.Default;

      return
        cmp1.Equals(operation, that.operation) &&
        cmp2.Equals(left, that.left) &&
        cmp2.Equals(right, that.right);
    }

    public bool Equals(IExpression<int> _that) {
      var that = _that as Arithmetic;
      return null != that && Equals(that);
    }
  }
}
