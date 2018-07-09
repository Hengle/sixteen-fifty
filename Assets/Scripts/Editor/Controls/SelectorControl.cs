using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

/**
  * \brief
  * Used to draw a drop-down menu with choices.
  *
  * This class contains state to represent the currently selected choice.
  *
  * There are two primary ways of interacting with the selector.
  * 1. Registering a listener on #Changed to be notified when the
  *    selected index changes.
  * 2. Monitoring #IsChanged, which becomes true on the frames where a
  *    change occurs.
  */
[Serializable]
public class SelectorControl {
  [SerializeField]
  protected int selected;

  /**
    * \brief
    * Gets or sets the selected index in the choices array that was
    * passed to the constructor, or `-1` if nothing is selected yet.
    *
    * To *deselect*, assign `-1`.
    */
  public int Selected {
    get {
      return selected - 1;
    }
    set {
      selected = value + 1;
    }
  }

  /**
   * \brief
   * Raised when the selected index changes.
   *
   * The newly selected index is the parameter.
   *
   * Note that this event is raised during #Draw.
   */
  public event Action<int> Changed;

  /**
   * \brief
   * Was the selected index changed in the last frame?
   */
  public bool IsChanged {
    get;
    protected set;
  }

  [SerializeField]
  protected string[] choices;

  [SerializeField]
  protected string label;

  public SelectorControl(string label, string[] choices, int initialChoice = -1) {
    this.choices = new [] { "Select" }.Concat(choices).ToArray();
    this.label = label;

    selected = initialChoice + 1;
    IsChanged = false;
  }

  /**
   * \brief
   * Draws the selector, and returns a boolean indicating whether the
   * selection changed.
   */
  public bool Draw() {
    var old = selected;
    selected = EditorGUILayout.Popup(label, selected, choices);
    IsChanged = old != selected;
    if(IsChanged)
      Changed?.Invoke(Selected);
    return IsChanged;
  }
}
