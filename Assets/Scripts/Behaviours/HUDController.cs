using UnityEngine;
using UnityEngine.UI;

namespace SixteenFifty.Behaviours {
  public class HUDController : MonoBehaviour {
    public PlayerMenuController playerMenuController;
    public Button inventoryButton;

    void OnInventoryButtonClick() {
      var inv = playerMenuController.hexGridManager.Player.GetComponent<HasInventory>();
      Debug.Assert(
        null != inv,
        "the current player has an inventory.");

      playerMenuController.inventoryController.inventory = inv.inventory;

      playerMenuController.gameObject.SetActive(true);
    }

    void OnEnable() {
      inventoryButton.onClick.AddListener(OnInventoryButtonClick);
    }

    void OnDisable() {
      inventoryButton.onClick.RemoveListener(OnInventoryButtonClick);
    }
  }
}
