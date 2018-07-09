using System;

namespace SixteenFifty.EventItems {
  using Commands;
  using Expressions;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Condition")]
  public class Condition : IScript {
    public IExpression<bool> condition;

    public IScript ifTrue;
    public IScript ifFalse;

    public Command<object> GetScript(EventRunner runner) =>
      Command<bool>.Pure(
        () => condition.Compute(runner))
      .Branch(
        ifTrue.GetScript(runner),
        ifFalse.GetScript(runner));
  }
}
