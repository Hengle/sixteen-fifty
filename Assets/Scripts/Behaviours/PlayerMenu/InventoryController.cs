using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
  public GameObject itemSlotPrefab;
  public const int MAX_INVENTORY_SIZE = 21;

  public GameObject inventoryArea;

  private ItemSlot[] slots;

  void CreateInventory(int size) {
    Debug.Assert(slots == null, "there is no inventory");
    slots = new ItemSlot[size];
    for(int i = 0; i < size; i++) {
      var obj = Instantiate(itemSlotPrefab, inventoryArea.transform);
      slots[i] = obj.GetComponent<ItemSlot>();
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
