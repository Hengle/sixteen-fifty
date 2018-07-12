using System;

using UnityEngine;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;

  [SubtypeEditorFor(target = typeof(Arithmetic))]
  public class ArithmeticEditor : ISubtypeEditor<IExpression<int>> {
    public SubtypeControl<IArithmeticOperation> operationSelector;
    public ExpressionControl<int> leftSelector;
    public ExpressionControl<int> rightSelector;

    [SerializeField]
    SubtypeSelectorContext<IExpression<int>> context;

    [SerializeField]
    Arithmetic target;

    public ArithmeticEditor(SubtypeSelectorContext<IExpression<int>> context) {
      this.context = context;
    }

    public bool CanEdit(Type type) => type == typeof(Arithmetic);

    public void Draw(IExpression<int> _target) {
      target = _target as Arithmetic;
      Debug.Assert(
        null != target,
        "ArithmeticEditor target is Arithmetic.");

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
        operationSelector ?? new SubtypeControl<IArithmeticOperation>(
          "Operation",
          new SubtypeSelectorContext<IArithmeticOperation>(),
          false);
      leftSelector =
        leftSelector ?? new ExpressionControl<int>(
          "Left operand",
          context);
      rightSelector =
        rightSelector ?? new ExpressionControl<int>(
          "Right operand",
          context);
    }
  }
}
