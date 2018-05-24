using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
  public PlayerMenuController playerMenuController;
  public Button inventoryButton;

  void OnInventoryButtonClick() {
    playerMenuController.gameObject.SetActive(true);
  }

  void OnEnable() {
    inventoryButton.onClick.AddListener(OnInventoryButtonClick);
  }

  void OnDisable() {
    inventoryButton.onClick.RemoveListener(OnInventoryButtonClick);
  }
}
