using System;

using UnityEngine;

namespace SixteenFifty {
  [Serializable]
  public class InventorySlot {
    /**
     * \brief
     * Constructs an empty InventorySlot.
     */
    public static InventorySlot Empty =>
      new InventorySlot(null, 0);

    public Item item;
    public int count;

    public int Room {
      get {
        Debug.Assert(null != item, "there is an item");
        return item.stackingSize - count;
      }
    }

    public InventorySlot(Item item, int count) {
      this.item = item;
      this.count = count;
    }

    /**
    * \brief
    * Produces a textual representation of the count of items in the
    * slot.
    *
    * If there is no item in the slot, the the text is empty.
    * If the Item in the slot has a Item#stackingSize of 1, then the
    * text is also empty.
    */
    public string CountText {
      get {
        if(null == item)
          return "";
        if(item.stackingSize == 1)
          return "";
        else
          return count.ToString();
      }
    }
  }
}
