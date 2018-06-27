using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SixteenFifty.UI {
  public class InventoryController : MonoBehaviour {
    public GameObject itemSlotPrefab;
    public GameObject inventoryArea;
    public PlayerMenuController playerMenuController;
    public Inventory inventory;

    public ItemSlotController[] Slots {
      get;
      private set;
    }

    void CreateInventory(int size) {
      Debug.Assert(Slots == null, "there is no inventory");
      Slots = new ItemSlotController[size];
      for(int i = 0; i < size; i++) {
        var obj = Instantiate(itemSlotPrefab, inventoryArea.transform);
        Slots[i] = obj.GetComponent<ItemSlotController>();
        Slots[i].BackingSlot = inventory.Slots[i];
      }
    }

    void DestroyInventory() {
      Debug.Assert(null != Slots, "there is an inventory to destroy");
      foreach(var slot in Slots) {
        Destroy(slot.gameObject);
      }
      Slots = null;
    }

    void OnEnable() {
      inventory = StateManager.Instance.playerController.inventory;
      Debug.Assert(null != inventory, "there is an inventory to display");
      CreateInventory(inventory.Size);
      playerMenuController.title.text = "Inventory";
    }

    void OnDisable() {
      inventory = null;
      DestroyInventory();
    }

    void Start () {

    }
  }
}
