using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Serialization;
  
  public class ActivateInteractableFocus : MonoBehaviour {
    [SerializeField] [HideInInspector]
    HexGridManager manager;

    [SerializeField] [HideInInspector]
    InteractableFocus focus;

    void Awake() {
      manager = GetComponentInParent<HexGridManager>();
      focus = GetComponentInParent<InteractableFocus>();

      Debug.Assert(
        null != focus,
        "ActivateInteractableFocus is instantiated under InteractableFocus.");
    }

    bool IsActive => !manager.eventManager.IsUI;

    void Update() {
      if(null == focus?.focus || !IsActive)
        return;

      if(Input.GetButtonUp("Fire1")) {
        Debug.Log("yo");
        manager.PresentInteractionsMenu(focus.focus.interactions);
      }
    }
  }
}
