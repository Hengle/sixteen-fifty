using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour {
  /**
   * \brief
   * Used to display the icon in the slot.
   */
  public Image iconRenderer;

  private InventorySlot slot;
  public InventorySlot BackingSlot {
    get {
      return slot;
    }
    set {
      slot = value;
      UpdateSprite();
    }
  }

  public void UpdateSprite() {
    iconRenderer.enabled = BackingSlot != null && BackingSlot.item != null;
    if(iconRenderer.enabled)
      iconRenderer.sprite = BackingSlot.item.icon;
  }

  /**
   * \brief
   * Computes how many more of the current item could be added to this
   * slot.
   *
   * Requires that #BackingSlot be not `null` and that its internal
   * item be not `null`.
   */
  public int Room {
    get {
      Debug.Assert(null != BackingSlot, "backing slot is not null");
      return BackingSlot.Room;
    }
  }

  /**
   * Clears the ItemSlot.
   * This is the same as nulling out the Item property.
   */
  public void Clear() {
    BackingSlot = null;
  }

  void Start () {
    UpdateSprite();
  }
}
