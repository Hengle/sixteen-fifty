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
    * Gets the selected index in the choices array that was passed to
    * the constructor, or -1 if nothing is selected yet.
    */
  public int Selected => selected - 1;

  public event Action<int> Changed;

  [SerializeField]
  string[] choices;

  [SerializeField]
  string label;

  public SelectorControl(string label, string[] choices) {
    this.choices = new [] { "Select" }.Concat(choices).ToArray();
    this.label = label;

    selected = 0;
  }

  public void Draw() {
    var old = selected;
    selected = EditorGUILayout.Popup(label, selected, choices);
    if(old != selected)
      Changed?.Invoke(Selected);
  }
}
