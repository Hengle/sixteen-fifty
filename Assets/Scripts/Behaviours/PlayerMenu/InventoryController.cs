using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SixteenFifty.Behaviours {
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
      Slots =
        inventory.slots.Select(
          slot => {
            var obj = Instantiate(itemSlotPrefab, inventoryArea.transform);
            var slotController = obj.GetComponent<ItemSlotController>();
            slotController.BackingSlot = slot;
            return slotController;
          })
        .ToArray();
    }

    void DestroyInventory() {
      Debug.Assert(null != Slots, "there is an inventory to destroy");
      foreach(var slot in Slots) {
        Destroy(slot.gameObject);
      }
      Slots = null;
    }

    void OnEnable() {
      Debug.Assert(null != inventory, "there is an inventory to display");
      CreateInventory(inventory.Size);
      playerMenuController.title.text = "Inventory";
    }

    void OnDisable() {
      inventory = null;
      DestroyInventory();
    }
  }
}
