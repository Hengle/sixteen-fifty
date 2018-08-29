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

    public bool Draw(IExpression<int> _target) {
      target = _target as Arithmetic;
      Debug.Assert(
        null != target,
        "ArithmeticEditor target is Arithmetic.");

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
