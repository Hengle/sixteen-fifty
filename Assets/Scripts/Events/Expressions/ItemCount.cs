using System;
using System.Linq;

namespace SixteenFifty.EventItems.Expressions {
  [Serializable]
  [SelectableSubtype(friendlyName = "Item count")]
  public class ItemCount : IExpression<int> {
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
  }
}
