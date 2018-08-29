using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [Serializable]
  [SubtypeEditorFor(target = typeof(GiveItem))]
  public class GiveItemEditor : ISubtypeEditor<IScript> {
    [SerializeField]
    GiveItem target;

    public GiveItemEditor(SubtypeSelectorContext<IScript> context) {
    }

    public bool CanEdit(Type type) =>
      type == typeof(GiveItem);

    public bool Draw(IScript _target) {
      target = _target as GiveItem;
      Debug.Assert(
        null != target,
        "Target of GiveItemEditor is of type GiveItem.");

      var old1 = target.item;
      var b1 =
        old1 !=
        (target.item =
         EditorGUILayout.ObjectField(
           "Item type",
           target.item,
           typeof(Item),
           false)
         as Item);

      var old2 = target.count;
      var b2 =
        old2 !=
        (target.count =
         EditorGUILayout.IntField(
           "Count",
           target.count));

      var old3 = target.target;
      var b3 =
        old3 !=
        (target.target =
         EditorGUILayout.ObjectField(
           "Target inventory",
           target.target,
           typeof(Inventory),
           false)
         as Inventory);

      return b1 || b2 || b3;
    }
  }
}
