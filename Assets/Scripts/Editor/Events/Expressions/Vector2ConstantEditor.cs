using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems.Expressions;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Constant<Vector2>))]
  public class Vector2ConstantEditor : ConstantEditor<Vector2> {
    public Vector2ConstantEditor(SubtypeSelectorContext<IExpression<Vector2>> context) :
    base(context) {
    }

    public override void Draw(IExpression<Vector2> _target) {
      base.Draw(_target);

      target.value = EditorGUILayout.Vector2Field("Value", target.value);
    }
  }
}
