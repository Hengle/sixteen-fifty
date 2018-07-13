using System;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Reflection;
  
  /**
   * \brief
   * A dynamic control that combines an SubtypeSelectorControl<T> and an
   * associated ISubtypeEditor<T>.
   */
  [Serializable]
  public class SubtypeControl<T> where T : class {
    [SerializeField]
    SubtypeSelectorControl<T> selector;

    [SerializeField]
    ISubtypeEditor<T> editor;

    [SerializeField]
    string selectorLabel;

    [SerializeField]
    SubtypeSelectorContext<T> context;

    [SerializeField]
    bool drawEditor;

    public SubtypeControl(
      string selectorLabel,
      SubtypeSelectorContext<T> context,
      bool drawEditor = true) {

      this.selectorLabel = selectorLabel;
      this.context = context;
      this.drawEditor = drawEditor;
    }

    /**
     * \brief
     * Draws the control to edit the given target object.
     *
     * If there is a change in the selected event item type, then a
     * new target object will be instantiated and returned.
     * Otherwise, the input object will be returned.
     */
    public T Draw(T script) {
      if(null == selector)
        selector = new SubtypeSelectorControl<T>(selectorLabel, context);

      T target = script;
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
          target = selectedType?.Construct<T>();
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

      var wantsNoEditor = type.IsDefined(
        typeof(NoEditorNeeded),
        true);

      // if we don't need to worry about editors, then gtfo
      if(!drawEditor || wantsNoEditor)
        return target;

      // if we have no editor, or the current editor is inappropriate,
      if(editor == null || !editor.CanEdit(type)) {
        // then we need to construct a new one
        if(null == (editor = context.GetEditor(type))) {
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
      EditorGUI.indentLevel++;
      editor.Draw(target);
      EditorGUI.indentLevel--;
      return target;
    }
  }
}
