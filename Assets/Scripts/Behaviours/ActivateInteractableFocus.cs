using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Serialization;
  
  public class ActivateInteractableFocus : MonoBehaviour {
    [SerializeField] [HideInInspector]
    HexGridManager manager;

    [SerializeField] [HideInInspector]
    InteractableFocus focus;

    [SerializeField] [HideInInspector]
    IHexInput hexInput;

    void Awake() {
      manager = GetComponentInParent<HexGridManager>();
      focus = GetComponentInParent<InteractableFocus>();
      hexInput = GetComponent(typeof(IHexInput))
        as IHexInput;

      Debug.Assert(
        null != manager,
        "ActivateInteractableFocus is under a HexGridManager.");

      Debug.Assert(
        null != hexInput,
        "ActivateInteractableFocus is with an IHexInput.");

      Debug.Assert(
        null != focus,
        "ActivateInteractableFocus is instantiated under InteractableFocus.");
    }

    void OnEnable() {
      hexInput.SubmitPressed += OnSubmitPressed;
    }

    void OnDisable() {
      hexInput.SubmitPressed -= OnSubmitPressed;
    }

    void OnSubmitPressed() {
      if(null == focus?.focus)
        return;

      manager.PresentInteractionsMenu(focus.focus.interactions);
    }
  }
}
