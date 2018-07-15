using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(GiveItem))]
  public class GiveItemEditor : ScriptedEventItemEditor {
    [SerializeField]
    GiveItem target;

    public GiveItemEditor(SubtypeSelectorContext<IScript> context) {
    }

    public override bool CanEdit(Type type) =>
      type == typeof(GiveItem);

    public override void Draw(IScript _target) {
      target = _target as GiveItem;
      Debug.Assert(
        null != target,
        "Target of GiveItemEditor is of type GiveItem.");

      RecordChange("set item to give");
      target.item =
        EditorGUILayout.ObjectField(
          "Item type",
          target.item,
          typeof(Item),
          false)
        as Item;

      RecordChange("set count to give");
      target.count =
        EditorGUILayout.IntField(
          "Count",
          target.count);

      RecordChange("set target inventory");
      target.target =
        EditorGUILayout.ObjectField(
          "Target inventory",
          target.target,
          typeof(Inventory),
          false)
        as Inventory;
    }
  }
}
