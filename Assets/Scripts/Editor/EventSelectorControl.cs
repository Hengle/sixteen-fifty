using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Reflection;
  
  /**
   * \brief
   * A selector control specialized for selecting IScript
   * types.
   */
  [Serializable]
  public class EventSelectorControl : SelectorControl {
    public EventSelectorControl(string label) :
        base(label, ScriptedEventEditorContext.SupportedEventNames) {
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
    public void Update<T>() where T : IScript => UnsafeUpdate(typeof(T));

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
      var iscript = typeof(IScript);
      if(type != null && !iscript.IsAssignableFrom(type))
        throw new TypeMismatch(type, iscript);

      UnsafeUpdate(type);
    }
    
    /**
     * \brief
     * Internal machinery for implementing Update.
     */
    void UnsafeUpdate(Type eventType) {
      var supportedEvents =
        ScriptedEventEditorContext.SupportedEvents;
      Debug.Assert(
        null != supportedEvents,
        "Events are supported.");
      // Debug.LogFormat(
      //   "Looking for {0} in list: {1}.",
      //   eventType,
      //   String.Join(
      //     ", ",
      //     supportedEvents.Select(e => e.ToString())));
          
      Selected = Array.IndexOf(
        supportedEvents,
        eventType);
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
        Debug.Assert(
          // why -1? There's an extra choice in the selector for null.
          choices.Length - 1 == ScriptedEventEditorContext.SupportedEvents.Length,
          "Number of choices in EventSelectorControl matches number of supported events.");
        if(Selected == -1)
          return null;
        else
          return ScriptedEventEditorContext.SupportedEvents[Selected];
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
    public ScriptedEventItemEditor InstantiateEditor() {
      var i = Selected;
      if(i == -1)
        return null;
      return ScriptedEventEditorContext.GetEditor(i);
    }

    /**
     * \brief
     * Creates a new event item according to the selected type.
     *
     * \returns
     * The new event item object, or null if there is currently no
     * selected event item class.
     */
    public T Instantiate<T>() where T : class, IScript {
      var i = Selected;
      if(i == -1)
        return null;
      return ScriptedEventEditorContext.SupportedEvents[i].DefaultConstruct<T>();
    }
  }
}
