using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Reflection;
  
  /**
   * \brief
   * A dynamic control that combines an EventSelectorControl and an
   * associated ScriptedEventItemEditor.
   */
  [Serializable]
  public class EventItemControl {
    [SerializeField]
    EventSelectorControl selector;

    [SerializeField]
    ScriptedEventItemEditor editor;

    [SerializeField]
    string selectorLabel;

    public EventItemControl(string selectorLabel) {
      this.selectorLabel = selectorLabel;
    }

    /**
     * \brief
     * Draws the control to edit the given target object.
     *
     * If there is a change in the selected event item type, then a
     * new target object will be instantiated and returned.
     * Otherwise, the input object will be returned.
     */
    public IScript Draw(IScript script) {
      if(null == selector)
        selector = new EventSelectorControl(selectorLabel);

      IScript target = script;
      var type = target?.GetType();

      // make the selector select the right thing.
      selector.Update(type);

      // if the selection has changed, or there is no editor, or the
      // current editor cannot edit the target script, then we need to
      // instantiate a new editor, but only if we *should* instantiate
      // one.
      if(selector.Draw()) {
        // if there is no selected type, then the event item is
        // dead.
        var selectedType = selector.SelectedType;
        if(selectedType == null) {
          return null;
        }

        if(selectedType != type) {
          target = selectedType?.DefaultConstruct<IScript>();
          Debug.Assert(
            null != target,
            "Target object exists after default construction.");
          type = selectedType;
        }
      }
      else if(type == null)
        return null;

      // from here on, we're sure that:
      // - type is not null
      // - target is not null
      // - type is the type of target
      // with this, we can construct and draw the editor for the
      // target.

      // if we have no editor, or the current editor is inappropriate,
      if(editor == null || !editor.CanEdit(type)) {
        // then we need to construct a new one
        if(null == (editor = ScriptedEventEditorContext.GetEditor(type))) {
          // but if after construction it's still null, that means
          // that we couldn't find an appropriate editor, so we draw a
          // warning in the inspector.
          EditorGUILayout.LabelField(
            String.Format(
              "No editor for type {0}.", type));
          return target;
        }

        Debug.Assert(
          editor.CanEdit(type),
          "Newly created editor can edit the target object.");
      }

      // from here on, we're certain that:
      // - editor is not null
      // - editor can edit target

      // finally we can edit the target, and return it.
      editor.DrawInspector(target);
      return target;
    }
  }
}
