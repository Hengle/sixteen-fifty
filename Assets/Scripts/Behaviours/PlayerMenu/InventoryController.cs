using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour {
  public GameObject itemSlotPrefab;
  public GameObject inventoryArea;
  public PlayerMenuController playerMenuController;

  public const int MAX_INVENTORY_SIZE = 21;

  public ItemSlot[] Slots {
    get;
    private set;
  }

  public IEnumerable<Numbered<ItemSlot>> NumberedSlots {
    get {
      Debug.Assert(null != Slots, "there is a slots array");
      return Slots.Numbering();
    }
  }

  public IEnumerable<Numbered<ItemSlot>> NonemptySlots {
    get {
      return NumberedSlots.Where(s => s.value.Item != null);
    }
  }

  public IEnumerable<Numbered<ItemSlot>> EmptySlots {
    get {
      return NumberedSlots.Where(s => s.value.Item == null);
    }
  }

  /**
   * \brief
   * Gets the first empty ItemSlot in the inventory.
   * Returns `null` if the inventory is full.
   */
  public Numbered<ItemSlot> GetEmptySlot() {
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
   * Finds the ItemSlot objects housing the given Item.
   */
  public IEnumerable<Numbered<ItemSlot>> FindItem(Item item) {
    return NonemptySlots
      // XXX is this the right equality?
      .Where(s => s.value.Item == item);
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
    if(count == 0)
      return true;

    if(count > RoomFor(item))
      return false;

    foreach(var s in FindItem(item)) {
      // compute the amount to adjust by
      int d = Math.Min(count, s.value.Room);
      count -= d;
      s.value.count += d;
      if(count == 0)
        return true;
    }

    foreach(var s in EmptySlots) {
      s.value.Item = item;
      s.value.count = 0;
      int d = Math.Min(count, item.stackingSize);
      count -= d;
      s.value.count += d;
      if(count == 0)
        return true;
    }

    Debug.Assert(count == 0, "added all items");
    return true;
  }

  void CreateInventory(int size) {
    Debug.Assert(Slots == null, "there is no inventory");
    Slots = new ItemSlot[size];
    for(int i = 0; i < size; i++) {
      var obj = Instantiate(itemSlotPrefab, inventoryArea.transform);
      Slots[i] = obj.GetComponent<ItemSlot>();
    }
  }

  void Awake() {
    CreateInventory(MAX_INVENTORY_SIZE);
  }

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
