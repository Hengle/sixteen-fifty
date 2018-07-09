using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;
  using Reflection;
  
  /**
   * \brief
   * A selector control specialized for selecting subtypes of a given
   * type `T`.
   */
  [Serializable]
  public class SubtypeSelectorControl<T> : SelectorControl where T : class {
    [SerializeField]
    SubtypeSelectorContext<T> context;

    public SubtypeSelectorControl(string label, SubtypeSelectorContext<T> context) :
        base(label, context.SelectableTypeNames) {
      this.context = context;
    }

    /**
     * \brief
     * Selects the appropriate index for `eventType` in the SelectorControl.
     *
     * \remark
     * If the given type does not exist with
     * ScriptedEventEditorContext.SupportedEvents, then the selected
     * index of this EventSelectorControl will become `-1`, i.e. "no
     * selection".
     *
     * Hence, to deselect, it suffices to call `Update(null)`.
     *
     * \param T The event type to select.
     */
    public void Update<S>() where S : T => UnsafeUpdate(typeof(S));

    /**
     * \brief
     * Selects the appropriate index for `eventType` in the SelectorControl.
     *
     * It is preferable to call the generic version of this method,
     * since it statically checks that the type implements IScript
     * using a generic constraint, whereas this method performs a
     * dynamic check using reflection, and will *throw an exception*
     * if the type is not assignable to IScript.
     *
     * \remark
     * If the given type does not exist with
     * ScriptedEventEditorContext.SupportedEvents, then the selected
     * index of this EventSelectorControl will become `-1`, i.e. "no
     * selection".
     *
     * Hence, to deselect, it suffices to call `Update(null)`.
     *
     * \param eventType The event type to select.
     */
    public void Update(Type type) {
      var t = typeof(T);
      if(type != null && !t.IsAssignableFrom(type))
        throw new TypeMismatch(type, t);

      UnsafeUpdate(type);
    }
    
    /**
     * \brief
     * Internal machinery for implementing Update.
     */
    void UnsafeUpdate(Type type) {
      var selectableTypes = context.SelectableTypes;
      Debug.Assert(
        null != selectableTypes,
        "There are some supported types are supported.");
          
      Selected = Array.IndexOf(
        selectableTypes,
        type);
    }

    /**
     * \brief
     * Decides whether instantiation of an editor or IScript class
     * should take place.
     *
     * This is true if and only if there is actually something selected.
     */
    public bool ShouldInstantiate =>
      Selected != -1;

    public Type SelectedType {
      get {
        // why -1? There's an extra choice in the selector for null.
        var l1 = choices.Length - 1;
        var l2 = context.SelectableTypes.Length;
        Debug.Assert(
          l1 == l2,
          String.Format(
            "Number of choices {0} in EventSelectorControl " +
            "matches number of supported events {1}.",
            l1, l2));
        if(Selected == -1)
          return null;
        else
          return context.SelectableTypes[Selected];
      }
    }

    /**
     * \brief
     * Creates an editor for the selected event item type.
     *
     * \returns
     * The ScriptedEventItemEditor for the selected event type.
     * If there is no selected event type, then returns null.
     */
    public ISubtypeEditor<T> InstantiateEditor() {
      var i = Selected;
      if(i == -1)
        return null;
      return context.GetEditor(i);
    }

    /**
     * \brief
     * Creates a new event item according to the currently selected
     * type.
     *
     * \returns
     * The new event item object, or null if there is currently no
     * selected event item class.
     *
     * \exception TypeMismatch
     * This is raised if the currently selected type is in fact not
     * `S`.
     */
    public S Instantiate<S>() where S : class, T {
      var i = Selected;
      if(i == -1)
        return null;
      return context.SelectableTypes[i].Construct<S>();
    }
  }
}
