using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class InteractionMenu : MonoBehaviour {
  public GameObject buttonPrefab;

  public Canvas Canvas {
    get;
    private set;
  }

  public CanvasGroup CanvasGroup {
    get;
    private set;
  }

  public Image Panel {
    get;
    private set;
  }

  public float Alpha {
    get {
      return CanvasGroup.alpha;
    }
    set {
      CanvasGroup.alpha = value;
    }
  }

  public bool MenuActive {
    get {
      Debug.Assert(CanvasGroup.interactable == CanvasGroup.blocksRaycasts);
      return CanvasGroup.interactable;
    }
    set {
      CanvasGroup.interactable = CanvasGroup.blocksRaycasts = value;
    }
  }

  private IList<InteractionButton> buttons;

  /**
   * Raised when any of the buttons are activated.
   */
  public event Action<Interaction> Interacted;

  void Awake() {
    Canvas = this.GetComponentNotNull<Canvas>();
    CanvasGroup = this.GetComponentInChildrenNotNull<CanvasGroup>();
    buttons = new List<InteractionButton>();
    Panel = this.GetComponentInChildrenNotNull<Image>();
  }

  void Start () {
    StateManager.Instance.eventManager.interactionMenu = this;
  }

  public void CreateMenu(IEnumerable<Interaction> interactions) {
    // If the count is nonzero, it means that the last person to make
    // a menu didn't clean themselves up properly
    Debug.Assert(0 == buttons.Count, "There are no interactions when creating a new menu.");

    Debug.Assert(null != buttonPrefab, "The button prefab is not null when creating a new menu.");
    
    foreach(var interaction in interactions) {
      buttons.Add(CreateInteractionButton(interaction));
    }
  }

  public void DestroyMenu() {
    foreach(var button in buttons) {
      button.Interacted -= OnButtonInteracted;
      Destroy(button);
    }
  }

  InteractionButton CreateInteractionButton(Interaction interaction) {
    var obj = Instantiate(buttonPrefab);
    obj.transform.SetParent(Panel.transform, false);
    var button = obj.GetComponent<InteractionButton>();
    button.Interaction = interaction;
    button.Interacted += OnButtonInteracted;
    // this subscription gets removed in DestroyMenu
    return button;
  }

  void OnButtonInteracted(Interaction interaction) {
    Debug.Assert(null != interaction.script, "Interaction script is not null.");
    if(null != Interacted) {
      Interacted(interaction);
    }
  }
}
