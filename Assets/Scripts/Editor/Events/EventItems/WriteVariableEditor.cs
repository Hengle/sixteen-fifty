using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  using Reflection;
  using Variables;

  [Serializable]
  [SubtypeEditorFor(target = typeof(WriteVariable))]
  public class WriteVariableEditor : ISubtypeEditor<IScript> {
    [SerializeField]
    WriteVariable target;

    // we don't know what the type of the expression is, so we can't
    // know what type to make the control, until much later.
    [SerializeField]
    object expressionControl;

    public WriteVariableEditor(SubtypeSelectorContext<IScript> context) {
    }

    public bool CanEdit(Type type) =>
      type == typeof(WriteVariable);

    public bool Draw(IScript _target) {
      target = _target as WriteVariable;

      Debug.Assert(
        null != target,
        "Target of WriteVariableEditor is of type WriteVariable.");

      var b = false;

      var old1 = target.destination;
      b = b ||
        old1 !=
        (target.destination =
         EditorGUILayout.ObjectField(
           "Variable",
           old1,
           typeof(AnyVariable),
           false)
         as ScriptableObject);

      // we don't even bother drawing the expression unless a variable
      // is selected; in fact if we *deselect* the variable, then we
      // should null out the expression.
      if(target.destination == null) {
        target.expression = null;
        return b;
      }

      // if a variable *is* selected, then we need to get out the type
      // of its contents in order to construct the appropriate
      // expression selector.
      var contentsType = target.GetVariableType();

      if(!ExpressionControlIsAppropriateFor(contentsType)) {
        CreateAppropriateExpressionControl(contentsType);
      }

      Debug.Assert(
        null != expressionControl,
        "There is an appropriate expression control.");

      var expressionControlType = expressionControl.GetType();
      var args = new object[] { target.expression };
      b = b ||
        (bool)expressionControlType.GetMethod("Draw").Invoke(
          expressionControl,
          args);
      target.expression = args[0];

      return b;
    }

    private Type GetExpressionControlTypeFor(Type contentsType) =>
      typeof(ExpressionControl<>).MakeGenericType(new [] { contentsType });

    private bool ExpressionControlIsAppropriateFor(Type contentsType) {
      if(expressionControl == null)
        return false;
      
      return
        expressionControl.GetType() ==
        GetExpressionControlTypeFor(contentsType);
    }

    private void CreateAppropriateExpressionControl(Type contentsType) {
      var subtypeSelectorContextType =
        typeof(SubtypeSelectorContext<>).MakeGenericType(
          new [] {
            typeof(IExpression<>).MakeGenericType(
              new [] { contentsType })
          });
      var subtypeSelectorContext =
        subtypeSelectorContextType.Construct<object>();

      var controlType = GetExpressionControlTypeFor(contentsType);
      expressionControl =
        controlType.GetConstructor(
          new [] { typeof(string), subtypeSelectorContextType })
        .Invoke(new object[] { "Expression", subtypeSelectorContext });
    }
  }
}
