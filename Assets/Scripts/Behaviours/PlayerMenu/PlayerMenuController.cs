using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace SixteenFifty.Behaviours {
  public class PlayerMenuController : MonoBehaviour {
    /**
     * Set in inspector.
     */
    public HUDController hudController;

    /**
     * Set in inspector.
     */
    public Button closeButton;

    /**
     * Set in inspector.
     */
    public TextMeshProUGUI title;

    /**
     * Set in inspector.
     */
    public HexGridManager hexGridManager;

    /**
     * Set in inspector.
     */
    public InventoryController inventoryController;

    void Awake() {
      Action<object, string> assertExists = (obj, name) =>
        Debug.Assert(
          null != obj,
          String.Format("The {0} exists.", name));

      assertExists(hudController, "HUD controller");
      assertExists(closeButton, "close button");
      assertExists(title, "title text");
      assertExists(hexGridManager, "hex grid manager");
      assertExists(inventoryController, "inventory controller");
    }

    void OnEnable () {
      hudController.gameObject.SetActive(false);
      closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    void OnDisable () {
      closeButton.onClick.RemoveListener(OnCloseButtonClick);
      hudController.gameObject.SetActive(true);
    }

    void OnCloseButtonClick() {
      this.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {

    }
  }
}
