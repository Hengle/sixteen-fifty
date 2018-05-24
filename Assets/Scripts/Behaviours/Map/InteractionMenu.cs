using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * \brief
 * A menu with buttons that trigger Interaction objects.
 *
 * An interaction menu is a menu with a list of buttons with
 * associated Interactions. Clicking a button causes the associated
 * Interaction's EventScript to be played.
 *
 * The InteractionMenu MonoBehaviour requires also a Canvas
 * MonoBehaviour to be attached to the GameObject. It's inside this
 * Canvas that the UI elements are drawn to construct the menu.
 * In particular, the GameObject with an InteractionMenu behaviour
 * must have a unique child GameObject with the CanvasGroup behaviour
 * and that GameObject must have a unique child with the Panel behaviour.
 */
[RequireComponent(typeof(Canvas))]
public class InteractionMenu : MonoBehaviour {
  /**
   * \brief
   * The prefab used to instantiate the buttons.
   */
  public GameObject buttonPrefab;

  /**
   * \brief
   * The Canvas into which the UI elements of the menu are drawn.
   * Initialized on Awake.
   */
  public Canvas canvas;

  /**
   * \brief
   * The CanvasGroup behaviour of the child of the GameObject.
   */
  public CanvasGroup canvasGroup;

  /**
   * \brief
   * The Panel behaviour of the grandchild of the GameObject.
   */
  public Image panel;

  /**
   * \brief
   * The opacity of the InteractionMenu.
   */
  public float Alpha {
    get {
      return canvasGroup.alpha;
    }
    set {
      canvasGroup.alpha = value;
    }
  }

  /**
   * \brief
   * Whether the InteractionMenu is active.
   *
   * The menu is active if CanvasGroup::interactable is `true`.
   * Furthermore, this property maintains the invariant that
   * CanvasGroup::interactable if and only if
   * CanvasGroup::blocksRaycasts.
   */
  public bool MenuActive {
    get {
      Debug.Assert(canvasGroup.interactable == canvasGroup.blocksRaycasts);
      return canvasGroup.interactable;
    }
    set {
      canvasGroup.interactable = canvasGroup.blocksRaycasts = value;
    }
  }

  private IList<InteractionButton> buttons;

  /**
   * \brief
   * Raised when any of the buttons are activated.
   * The Interaction associated with the Button is passed to the event
   * handler.
   */
  public event Action<Interaction> Interacted;

  void Awake() {
    buttons = new List<InteractionButton>();
  }

  /**
   * \brief
   * Populates the menu with the given collection of Interactions.
   *
   * You *must* eveventally call DestroyMenu before calling CreateMenu
   * again.
   */
  public void CreateMenu(IEnumerable<Interaction> interactions) {
    // If the count is nonzero, it means that the last person to make
    // a menu didn't clean themselves up properly
    Debug.Assert(0 == buttons.Count, "There are no interactions when creating a new menu.");

    // we need the prefab in order to instantiate the buttons!
    Debug.Assert(null != buttonPrefab, "The button prefab is not null when creating a new menu.");
    
    var offset = buttonPrefab.transform.position.y;
    foreach(var interaction in interactions) {
      var button = CreateInteractionButton(interaction);
      button.transform.position += new Vector3(0f, offset, 0f);
      offset -= (button.transform as RectTransform).rect.height;
      buttons.Add(button);
    }
  }

  /**
   * \brief
   * Cleans up the buttons in the menu.
   */
  public void DestroyMenu() {
    foreach(var button in buttons) {
      button.Interacted -= OnButtonInteracted;
      Destroy(button);
    }
    buttons.Clear();
  }

  InteractionButton CreateInteractionButton(Interaction interaction) {
    var obj = Instantiate(buttonPrefab);
    obj.transform.SetParent(panel.transform, false);
    var button = obj.GetComponent<InteractionButton>();
    button.Interaction = interaction;
    button.Interacted += OnButtonInteracted;
    // this subscription gets removed in DestroyMenu
    return button;
  }

  /**
   * \brief
   * Handles button presses by raising Interacted, thus passing the
   * event up the chain.
   */
  void OnButtonInteracted(Interaction interaction) {
    Debug.Assert(null != interaction.script, "Interaction script is not null.");
    if(null != Interacted) {
      Interacted(interaction);
    }
  }
}
