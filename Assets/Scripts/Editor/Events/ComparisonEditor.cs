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

    public void Draw(IExpression<bool> _target) {
      target = _target as Comparison;
      Debug.Assert(
        null != target,
        "ComparisonEditor target is Comparison");

      InstantiateSelectors();

      target.operation =
        operationSelector.Draw(target.operation);
      target.left =
        leftSelector.Draw(target.left);
      target.right =
        rightSelector.Draw(target.right);
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
