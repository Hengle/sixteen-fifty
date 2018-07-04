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
      if(selector.Draw() || editor == null || !editor.CanEdit(type)) {
        if(selector.ShouldInstantiate) {
          // if after instantiation there still isn't an editor, it's
          // because there just isn't one for the desired type!
          if(null == (editor = selector.InstantiateEditor())) {
            EditorGUILayout.LabelField(
              String.Format(
                "No editor for type `{0}`.",
                type));
            return target;
          }
        }
        else {
          editor = null;
          return target;
        }
      }

      // if there is no selected type, then the event item is
      // dead.
      var selectedType = selector.SelectedType;
      if(selectedType == null) {
        return null;
      }

      // if the selected type of event item differs from the current
      // target object, then we need to make a new target object.
      if(selectedType != type) {
        target = selectedType.DefaultConstruct<IScript>();
      }

      // finally we can edit the target, and return it.
      editor.DrawInspector(target);
      return target;
    }
  }
}
