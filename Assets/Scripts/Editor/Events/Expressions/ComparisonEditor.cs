using System;

using UnityEngine;

namespace SixteenFifty.Editor {
  using EventItems.Expressions;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Comparison))]
  public class ComparisonEditor : ISubtypeEditor<IExpression<bool>> {
    [SerializeField]
    Comparison target;

    public SubtypeControl<IComparisonOperator<int>> operationSelector;
    public ExpressionControl<int> leftSelector;
    public ExpressionControl<int> rightSelector;
    
    public ComparisonEditor(SubtypeSelectorContext<IExpression<bool>> context) {
    }

    public bool CanEdit(Type type) => type == typeof(Comparison);

    public bool Draw(IExpression<bool> _target) {
      target = _target as Comparison;
      Debug.Assert(
        null != target,
        "ComparisonEditor target is Comparison");

      InstantiateSelectors();

      var b = false;

      var op = target.operation;
      b = b || operationSelector.Draw(ref op);
      target.operation = op;

      var left = target.left;
      b = b || leftSelector.Draw(ref left);
      target.left = left;

      var right = target.right;
      b = b || rightSelector.Draw(ref right);
      target.right = right;

      return b;
    }

    void InstantiateSelectors() {
      operationSelector =
        operationSelector ?? new SubtypeControl<IComparisonOperator<int>>(
          "Operation",
          new SubtypeSelectorContext<IComparisonOperator<int>>(),
          false);
      leftSelector =
        leftSelector ?? new ExpressionControl<int>(
          "Left operand",
          new SubtypeSelectorContext<IExpression<int>>());
      rightSelector =
        rightSelector ?? new ExpressionControl<int>(
          "Right operand",
          new SubtypeSelectorContext<IExpression<int>>());
    }
  }
}
