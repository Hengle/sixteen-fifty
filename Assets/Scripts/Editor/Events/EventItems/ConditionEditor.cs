using System;

using UnityEngine;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(Condition))]
  public class ConditionEditor : ISubtypeEditor<IScript> {
    [SerializeField]
    ExpressionControl<bool> expressionControl;

    [SerializeField]
    EventItemControl ifTrueControl;

    [SerializeField]
    EventItemControl ifFalseControl;

    [SerializeField]
    Condition target;

    [SerializeField]
    SubtypeSelectorContext<IScript> context;

    public ConditionEditor(SubtypeSelectorContext<IScript> context) {
      this.context = context;
    }

    public bool Draw(IScript _target) {
      // set up controls if they don't exist.
      ifTrueControl = ifTrueControl ?? new EventItemControl("True", context);
      ifFalseControl = ifFalseControl ?? new EventItemControl("False", context);
      expressionControl =
        expressionControl ?? new ExpressionControl<bool>(
          "Condition",
          new SubtypeSelectorContext<IExpression<bool>>());
      
      target = _target as Condition;
      Debug.Assert(
        null != target,
        "ConditionEditor target is a Condition.");

      var tc = target.condition;
      var tt = target.ifTrue;
      var tf = target.ifFalse;

      var b1 = expressionControl.Draw(ref tc);
      var b2 = ifTrueControl.Draw(ref tt);
      var b3 = ifFalseControl.Draw(ref tf);

      target.condition = tc;
      target.ifTrue = tt;
      target.ifFalse = tf;

      return b1 || b2 || b3;
    }

    public bool CanEdit(Type type) =>
      type == typeof(Condition);
  }
}
