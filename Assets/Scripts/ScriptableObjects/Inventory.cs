using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SixteenFifty {
  [CreateAssetMenu(menuName = "1650/Inventory")]
  public class Inventory : ScriptableObject {
    public InventorySlot[] slots = new InventorySlot[0];

    /**
     * \brief
     * Computes the number of slots in the inventory.
     */
    public int Size => slots.Length;

    /**
     * \brief
     * Enumerates all inventory slots, numbering them.
     */
    public IEnumerable<Numbered<InventorySlot>> NumberedSlots {
      get {
        Debug.Assert(null != slots, "there is a slots array");
        return slots.Numbering();
      }
    }

    /**
     * \brief
     * Enumerates all nonempty inventory slots.
     */
    public IEnumerable<Numbered<InventorySlot>> NonemptySlots {
      get {
        return NumberedSlots.Where(s => s.value.item != null);
      }
    }

    /**
     * \brief
     * Enumerates all empty inventory slots.
     */
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
      return EmptySlots.FirstOrSentinel(null);
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
  }
}
