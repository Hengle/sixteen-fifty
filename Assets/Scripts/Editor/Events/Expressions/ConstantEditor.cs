using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  
  [Serializable]
  public class ConstantEditor<T> : ISubtypeEditor<IExpression<T>> {
    [SerializeField]
    protected SubtypeSelectorContext<IExpression<T>> context;

    [SerializeField]
    protected Constant<T> target;
    
    public ConstantEditor(SubtypeSelectorContext<IExpression<T>> context) {
      this.context = context;
    }

    public bool CanEdit(Type type) => type == typeof(Constant<T>);

    public virtual bool Draw(IExpression<T> _target) {
      target = _target as Constant<T>;
      Debug.Assert(
        null != target,
        "ConstantEditor<T> target is a Constant<T>.");

      return false;
    }
  }
}
