using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using EventItems.Expressions;
  using TileMap;

  [Serializable]
  [SubtypeEditorFor(target = typeof(Constant<HexCoordinates>))]
  public class HexCoordinatesConstantEditor : ConstantEditor<HexCoordinates> {
    public HexCoordinatesConstantEditor(
      SubtypeSelectorContext<IExpression<HexCoordinates>> context) :
    base(context) {
    }

    public override bool Draw(IExpression<HexCoordinates> _target) {
      var target = _target as Constant<HexCoordinates>;
      Debug.Assert(
        null != target,
        "HexCoordinatesConstantEditor target is Constant<HexCoordinates>.");

      var old = target.value;

      var x = EditorGUILayout.IntField(
        "X", target.value.X);
      var y = EditorGUILayout.IntField(
        "Y", target.value.Y);
      if(x != target.value.X || y != target.value.Y)
        target.value = new HexCoordinates(x, y, target.value.Metrics);
      EditorGUILayout.LabelField(
        "Coordinates",
        target.value.ToString());

      return !target.value.Equals(old);
    }
  }
}
