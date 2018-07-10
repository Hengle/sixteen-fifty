using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Constant<int>))]
  public class IntConstantEditor : ConstantEditor<int> {
    public IntConstantEditor(SubtypeSelectorContext<IExpression<int>> context) :
    base(context) {
    }
    public override void Draw(IExpression<int> _target) {
      // sets well-typed target
      base.Draw(_target);

      target.value = EditorGUILayout.DelayedIntField("value", target.value);
    }
  }
}
