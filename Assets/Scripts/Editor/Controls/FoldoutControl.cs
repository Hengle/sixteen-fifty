using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  /**
   * \brief
   * Maintains state for drawing a foldout.
   */
  [Serializable]
  public class FoldoutControl {
    [SerializeField]
    bool state;

    public bool State {
      get {
        return state;
      }
      private set {
        state = value;
      }
    }

    [SerializeField]
    string label;

    public FoldoutControl(string label, bool initialState = false) {
      this.label = label;
      state = initialState;
    }

    public bool Draw() =>
      state = EditorGUILayout.Foldout(state, label);
  }
}
