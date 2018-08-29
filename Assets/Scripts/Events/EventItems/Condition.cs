using System;
using System.Collections.Generic;

namespace SixteenFifty.EventItems {
  using Commands;
  using Expressions;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Condition")]
  public class Condition : IScript, IEquatable<Condition> {
    public IExpression<bool> condition;

    public IScript ifTrue;
    public IScript ifFalse;

    public Command<object> GetScript(EventRunner runner) =>
      Command<bool>.Pure(
        () => condition.Compute(runner))
      .Branch(
        ifTrue.GetScript(runner),
        ifFalse.GetScript(runner));

    public bool Equals(Condition that) {
      var cmp1 = EqualityComparer<IExpression<bool>>.Default;
      var cmp2 = EqualityComparer<IScript>.Default;
      return
        cmp1.Equals(condition, that.condition) &&
        cmp2.Equals(ifTrue, that.ifTrue) &&
        cmp2.Equals(ifFalse, that.ifFalse);
    }

    public bool Equals(IScript _that) {
      var that = _that as Condition;
      return
        null != that && Equals(that);
    }
  }
}
