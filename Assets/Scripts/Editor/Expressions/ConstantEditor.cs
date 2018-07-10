using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(Constant<>))]
  public class ConstantEditor<T> : ISubtypeEditor<IExpression<T>> {
    [SerializeField]
    SubtypeSelectorContext<IExpression<T>> context;

    [SerializeField]
    Constant<T> target;
    
    public ConstantEditor(SubtypeSelectorContext<IExpression<T>> context) {
      this.context = context;
    }

    public bool CanEdit(Type type) => type == typeof(Constant<T>);

    public void Draw(IExpression<T> _target) {
      target = _target as Constant<T>;
      Debug.Assert(
        null != target,
        "ConstantEditor<T> target is a Constant<T>.");
    }
  }
}
