using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems.Expressions;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(ItemCount))]
  public class ItemCountEditor : ISubtypeEditor<IExpression<int>> {
    public ItemCountEditor(SubtypeSelectorContext<IExpression<int>> context) {
    }

    public bool CanEdit(Type type) => type == typeof(ItemCount);

    public bool Draw(IExpression<int> _target) {
      var target = _target as ItemCount;

      var b = false;

      var old1 = target.inventory;
      target.inventory =
        EditorGUILayout.ObjectField(
          "Inventory",
          old1,
          typeof(Inventory),
          false)
        as Inventory;
      b = b || old1 != target.inventory;

      var old2 = target.item;
      target.item =
        EditorGUILayout.ObjectField(
          "Item",
          target.item,
          typeof(Item),
          false)
        as Item;
      b = b || old2 != target.item;

      return b;
    }
  }
}
