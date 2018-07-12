using System;

using UnityEngine;

namespace SixteenFifty.Editor {
  using EventItems.Expressions;
  
  public class ExpressionControl<T> : SubtypeControl<IExpression<T>> {
    public ExpressionControl(string selectorLabel, SubtypeSelectorContext<IExpression<T>> context) :
    base(selectorLabel, context) {
    }
  }
}
