using System;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Expressions;
  using Variables;
  
  [Serializable]
  [SelectableSubtype(friendlyName = "Set Variable")]
  public class WriteVariable : ImmediateScript {
    public ScriptableObject destination;
    public object expression;

    // in my dreams, the following reflection code would just be written as:
    // destination.Value = expression.Compute();

    public Type GetVariableType() {
      if(destination == null)
        return null;
      
      // the destination type might be something like
      // IntVariable : Variable<int>
      // so we need to enumerate its interfaces to find IVariable<T>
      // to fish out the T.
      var destinationType = destination.GetType();
      var ifaces = destinationType.GetInterfaces();
      foreach(var iface in ifaces) {
        if(iface.GetGenericTypeDefinition() == typeof(IVariable<>)) {
          return iface.GetGenericArguments()[0];
        }
      }

      throw new SixteenFiftyException("type isn't a variable");
    }

    public Type GetRequiredExpressionType() {
      return GetRequiredExpressionType(GetVariableType());
    }

    public Type GetRequiredExpressionType(Type varT) {
      Debug.Assert(
        varT != null,
        "Variable contents type is not null.");

      return
        typeof(IExpression<>).MakeGenericType(new [] { varT });
    }

    public override void Call(EventRunner runner) {
      if(null == expression || null == destination) {
        Debug.LogWarning(
          "Either a variable write operation's source expression " +
          "or destination variable are not set.");
        return;
      }

      var destinationType = destination.GetType();
      var varT = GetVariableType();
      // ^ the type of the value stored in the variable; let's call
      // this `T`.

      // form IExpression<T>.
      var requiredExpressionType = 
        GetRequiredExpressionType(varT);
      // check that the expression is assignable to IExpression<T>:
      var expressionType = expression.GetType();
      Debug.Assert(
        requiredExpressionType.IsAssignableFrom(expressionType),
        "Expression object is assignable to IExpression<T>.");

      // then we can call Compute
      var compute = expressionType.GetMethod("Compute");
      Debug.Assert(
        null != compute,
        "Expression implements the Compute method.");

      var result = compute.Invoke(expression, new object[] { runner });

      // next, we need to typecheck the result; the result type needs
      // to be assignable to the variable contents type.
      var resultType = result.GetType();
      Debug.Assert(
        varT.IsAssignableFrom(resultType),
        "Computed result can be assigned to variable.");

      // then we can do the assignment
      var valueProp = destinationType.GetProperty("Value");
      Debug.Assert(
        null != valueProp,
        "The destination variable implements the `Value` property.");

      valueProp.SetValue(destination, result);
    }
  }
}
