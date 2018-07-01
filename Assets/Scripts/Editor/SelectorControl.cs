using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

/**
  * \brief
  * Used to draw a drop-down menu with choices.
  *
  * This class contains state to represent the currently selected choice.
  */
[Serializable]
public class SelectorControl {
  [SerializeField]
  int selected;

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

  public event Action<int> Changed;

  [SerializeField]
  string[] choices;

  [SerializeField]
  string label;

  public SelectorControl(string label, string[] choices, int initialChoice = -1) {
    this.choices = new [] { "Select" }.Concat(choices).ToArray();
    this.label = label;

    selected = initialChoice + 1;
  }

  public void Draw() {
    var old = selected;
    selected = EditorGUILayout.Popup(label, selected, choices);
    if(old != selected)
      Changed?.Invoke(Selected);
  }
}
