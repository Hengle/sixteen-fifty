using System;
using System.Linq;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Item count")]
  public class ItemCount : IExpression<int>, IEquatable<ItemCount> {
    public Inventory inventory;
    public Item item;

    public int Compute(EventRunner runner) {
      if(item == null || inventory == null)
        return 0;
      
      return
        inventory.FindItem(item)
        .Select(
          ns => ns.value.count)
        .Sum();
    }

    public bool Equals(ItemCount that) =>
      inventory == that.inventory &&
      item == that.item;

    public bool Equals(IExpression<int> that) =>
      IEquatableExt.Equals(this, that);
  }
}
