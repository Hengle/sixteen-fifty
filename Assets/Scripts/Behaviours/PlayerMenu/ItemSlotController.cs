using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace SixteenFifty {
  public class ItemSlotController : MonoBehaviour {
    /**
    * \brief
    * Used to display the icon in the slot.
    */
    public Image iconRenderer;

    /**
    * \brief
    * Used to show the count of the item in the slot.
    */
    public TextMeshProUGUI countText;

    private InventorySlot slot;

    /**
    * \brief
    * The model associated with this controller.
    */
    public InventorySlot BackingSlot {
      get {
        return slot;
      }
      set {
        slot = value;
        Refresh();
      }
    }

    /**
    * \brief
    * Refreshes the view according to the model.
    */
    public void Refresh() {
      RefreshSprite();
      RefreshCount();
    }

    private void RefreshSprite() {
      iconRenderer.sprite = BackingSlot?.item?.icon;
      iconRenderer.enabled = iconRenderer.sprite != null;
    }

    private void RefreshCount() {
      if(null == BackingSlot)
        countText.text = "";
      else
        countText.text = BackingSlot.CountText;
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
      Refresh();
    }
  }
}
