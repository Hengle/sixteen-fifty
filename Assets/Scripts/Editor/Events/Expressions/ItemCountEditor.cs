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

    public void Draw(IExpression<int> _target) {
      var target = _target as ItemCount;

      target.inventory =
        EditorGUILayout.ObjectField(
          "Inventory",
          target.inventory,
          typeof(Inventory),
          false)
        as Inventory;

      target.item =
        EditorGUILayout.ObjectField(
          "Item",
          target.item,
          typeof(Item),
          false)
        as Item;
    }
  }
}
