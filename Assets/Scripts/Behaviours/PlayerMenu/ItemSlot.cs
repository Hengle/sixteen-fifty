using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour {
  Item item;

  /**
   * \brief
   * The count of how many of #Item there are in the slot.
   */
  public int count;

  public SpriteRenderer iconRenderer;

  /**
   * The Item shown in the slot.
   */
  public Item Item {
    get {
      return item;
    }
    set {
      item = value;
      iconRenderer.sprite = value.icon;
      // enable the renderer iff we're not nulling out the item.
      iconRenderer.enabled = value != null;
    }
  }

  /**
   * \brief
   * Computes how many more of the current Item could be added to this slot.
   *
   * Requires that #Item be not `null`.
   */
  public int Room {
    get {
      Debug.Assert(null != Item, "current item is not null");
      return Item.stackingSize - count;
    }
  }

  /**
   * Clears the ItemSlot.
   * This is the same as nulling out the Item property.
   */
  public void Clear() {
    Item = null;
  }

  void Start () {
    iconRenderer.enabled = item != null;
  }
}
