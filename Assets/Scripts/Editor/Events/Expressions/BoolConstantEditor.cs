using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Constant<bool>))]
  public class BoolConstantEditor : ConstantEditor<bool> {
    public BoolConstantEditor(SubtypeSelectorContext<IExpression<bool>> context) :
    base(context) {
    }

    public override void Draw(IExpression<bool> _target) {
      // sets well-typed target
      base.Draw(_target);

      target.value = EditorGUILayout.Toggle("value", target.value);
    }
  }
}
