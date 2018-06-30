using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty {
  public class Inventory : MonoBehaviour {
    /**
    * \brief
    * Raised when the inventory is changed.
    */
    public event Action<Inventory> Changed;

    public InventorySlot[] Slots {
      get;
      private set;
    }

    const int MAX_INVENTORY_SIZE = 21;

    /**
    * \brief
    * Gets the size of the inventory (number of slots).
    * If the inventory is uninitialized, this is zero.
    */
    public int Size {
      get {
        return null == Slots ? 0 : Slots.Length;
      }
    }

    void Awake() {
      Slots = new InventorySlot[MAX_INVENTORY_SIZE];
      for(int i = 0; i < Slots.Length; i++) {
        Slots[i] = new InventorySlot(null, 0);
      }
    }

    public IEnumerable<Numbered<InventorySlot>> NumberedSlots {
      get {
        Debug.Assert(null != Slots, "there is a slots array");
        return Slots.Numbering();
      }
    }

    public IEnumerable<Numbered<InventorySlot>> NonemptySlots {
      get {
        return NumberedSlots.Where(s => s.value.item != null);
      }
    }

    public IEnumerable<Numbered<InventorySlot>> EmptySlots {
      get {
        return NumberedSlots.Where(s => s.value.item == null);
      }
    }

    /**
    * \brief
    * Gets the first empty InventorySlot in the inventory.
    * Returns `null` if the inventory is full.
    */
    public Numbered<InventorySlot> GetEmptySlot() {
      return EmptySlots.FirstOrDefault(null);
    }

    /**
    * \brief
    * Checks whether the inventory is full, i.e. has no empty slots.
    */
    public bool IsFull() {
      return GetEmptySlot() == null;
    }

    /**
    * \brief
    * Finds the InventorySlot objects housing the given Item.
    */
    public IEnumerable<Numbered<InventorySlot>> FindItem(Item item) {
      // XXX is this the right equality?
      return NonemptySlots.Where(s => s.value.item == item);
    }

    /**
    * \brief
    * Computes the maximum count how many of the given Item could be
    * added to the inventory.

    * This takes into account non-full slots containing the given item
    * as well as empty slots and the stacking size of the item.
    * The `item` parameter must be not null.
    */
    private int RoomFor(Item item) {
      if(item == null) throw new ArgumentNullException("item");

      return FindItem(item).Select(s => s.value.Room).Sum()
        + EmptySlots.Select(_ => item.stackingSize).Sum();
    }

    /**
    * \brief
    * Adds the given count of the Item to the inventory.
    *
    * This method requires the addition to be completely successful in
    * order to work.
    * The return value indicates whether the addition happened.
    *
    * For example, suppose there are no empty inventory slots, but
    * there exists one slot containing the given Item with a count of
    * 9/10. The amount to add is 4. In this situation, the method will
    * return `false` and do nothing.
    */
    public bool AddItem(Item item, int count = 1) {
      if(count == 0) {
        Debug.Log("Adding item with count zero! Nothing to do.");
        return true;
      }

      if(count > RoomFor(item))
        return false;

      foreach(var s in FindItem(item)) {
        // compute the amount to adjust by
        int d = Math.Min(count, s.value.Room);
        count -= d;
        s.value.count += d;
        if(count == 0) {
          Debug.Log("Added all items to existing slots.");
          return true;
        }
      }

      foreach(var s in EmptySlots) {
        s.value.item = item;
        s.value.count = 0;
        int d = Math.Min(count, item.stackingSize);
        count -= d;
        s.value.count += d;
        if(count == 0) {
          Debug.Log("Added all items to empty slots.");
          break;
        }
      }

      Debug.Assert(count == 0, "added all items");
      return true;
    }

    private void RaiseChanged() {
      if(null != Changed) {
        Changed(this);
      }
    }
  }

  [Serializable]
  public class InventorySlot {
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
