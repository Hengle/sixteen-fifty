using System;

using UnityEngine;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(Condition))]
  public class ConditionEditor : ScriptedEventItemEditor {
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

    public override void Draw(IScript _target) {
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

      target.condition = expressionControl.Draw(target.condition);
      target.ifTrue = ifTrueControl.Draw(target.ifTrue);
      target.ifFalse = ifFalseControl.Draw(target.ifFalse);
    }

    public override bool CanEdit(Type type) =>
      type == typeof(Condition);
  }
}
