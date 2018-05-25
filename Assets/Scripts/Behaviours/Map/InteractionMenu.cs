using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * \brief
 * A menu with buttons that trigger Interaction objects.
 *
 * An interaction menu is a menu with a list of buttons with
 * associated Interaction objects. Clicking a button causes the
 * associated Interaction's EventScript to be played.
 *
 * How to use the menu: set #interactions to the list of interactions
 * to display, register a callback on #Interacted, and then enable the
 * GameObject with this component on it.
 */
public class InteractionMenu : MonoBehaviour {
  /**
   * \brief
   * The prefab used to instantiate the buttons.
   */
  public GameObject buttonPrefab;

  /**
   * \brief
   * The Panel behaviour of the grandchild of the GameObject.
   */
  public Image panel;

  /**
   * \brief
   * The button that closes the menu when clicked.
   */
  public Button closeButton;

  /**
   * \brief
   * The list of buttons that presently make up the menu.
   */
  private IList<InteractionButton> buttons;

  /**
   * \brief
   * Raised when any of the buttons are activated.
   * The Interaction associated with the Button is passed to the event
   * handler.
   * If the menu is dismissed by the player without selecting an
   * Interaction, then the Interaction argument will be `null`.
   */
  public event Action<Interaction> Interacted;

  /**
   * \brief
   * The interactions to display in the menu.
   */
  public Interaction[] interactions { get; set; }

  void Awake() {
  }

  void Start() {
    Debug.Assert(
      null != panel,
      "An image (panel) component is required.");
  }

  void OnEnable() {
    CreateMenu();
    closeButton.onClick.AddListener(OnCloseButtonClick);
  }

  void OnDisable() {
    closeButton.onClick.RemoveListener(OnCloseButtonClick);
    DestroyMenu();
  }

  public void Show(
    Interaction[] interactions,
    Action<Interaction> onInteracted) {

    Debug.Assert(
      !gameObject.activeInHierarchy, "there is no interaction menu");

    if(null == interactions)
      throw new ArgumentNullException("interactions");

    if(null == onInteracted)
      throw new ArgumentNullException("onInteracted");

    this.interactions = interactions;
    Interacted += onInteracted;
    gameObject.SetActive(true);
  }

  /**
   * \brief
   * Populates the menu with the contents of #interactions.
   *
   * You *must* eventally call #DestroyMenu before calling #CreateMenu
   * again. Generally, this doesn't need to be worried about because
   * #OnEnable calls #CreateMenu and #OnDisable calls #DestroyMenu,
   * and Unity guarantees that #OnEnable and #OnDisable are called in
   * pairs.
   */
  private void CreateMenu() {
    // If the count is nonzero, it means that the last person to make
    // a menu didn't clean themselves up properly
    Debug.Assert(null == buttons, "There are no interactions when creating a new menu.");

    // we need the prefab in order to instantiate the buttons!
    Debug.Assert(null != buttonPrefab, "The button prefab exists when creating a new menu.");

    buttons = interactions.Select(CreateInteractionButton).ToList();
  }

  /**
   * \brief
   * Cleans up the buttons in the menu.
   *
   * This method is idempotent.
   */
  private void DestroyMenu() {
    foreach(var button in buttons) {
      button.Interacted -= OnButtonInteracted;
      Destroy(button.gameObject);
    }
    buttons.Clear();
    buttons = null;
  }

  InteractionButton CreateInteractionButton(Interaction interaction) {
    var obj = Instantiate(buttonPrefab, panel.transform);
    var button = obj.GetComponent<InteractionButton>();
    button.Interaction = interaction;
    // this subscription gets removed in DestroyMenu
    button.Interacted += OnButtonInteracted;
    return button;
  }

  /**
   * \brief
   * Handles button presses by raising Interacted, thus passing the
   * event up the chain.
   */
  void OnButtonInteracted(Interaction interaction) {
    Debug.Assert(null != interaction.script, "Interaction script is not null.");
    gameObject.SetActive(false);
    if(null != Interacted) {
      Interacted(interaction);
    }
  }

  void OnCloseButtonClick() {
    gameObject.SetActive(false);
    if(null != Interacted) {
      Interacted(null);
    }
  }
}
