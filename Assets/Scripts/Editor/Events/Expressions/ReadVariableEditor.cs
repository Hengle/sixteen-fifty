using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  using Variables;

  [Serializable]
  [SubtypeEditorFor(target = typeof(ReadVariable<>))]
  public class ReadVariableEditor<T> : ISubtypeEditor<IExpression<T>> {
    ReadVariable<T> target;

    public ReadVariableEditor(SubtypeSelectorContext<IExpression<T>> context) {
    }
    
    public bool CanEdit(Type type) => type == typeof(ReadVariable<T>);

    public bool Draw(IExpression<T> _target) {
      target = _target as ReadVariable<T>;
      Debug.Assert(
        null != target,
        "ReadVariableEditor<T> target is a Variable<T>.");

      var old = target.variable;
      target.variable =
        EditorGUILayout.ObjectField(
          "Variable",
          old,
          typeof(Variable<T>),
          false)
        as Variable<T>;

      return old != target.variable;
    }
  }
}
