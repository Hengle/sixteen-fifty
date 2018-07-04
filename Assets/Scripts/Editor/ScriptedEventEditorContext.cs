using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Reflection;
  
  public static class ScriptedEventEditorContext {
    /**
     * \brief
     * Maps IScript types to their editors.
     */
    private static Dictionary<Type, Type> editors;

    /**
     * \brief
     * A list of all IScript implementations with the EventAttribute.
     */
    private static Type[] supportedEvents;
    public static Type[] SupportedEvents {
      get {
        if(null != supportedEvents)
          return supportedEvents;
        supportedEvents = GetSupportedEventTypes().ToArray();
        return supportedEvents;
      }
    }

    /**
     * \brief
     * A list of the names of all supported events.
     *
     * This array is basically just used for reducing memory usage
     * when creating SelectorControl objects for choosing an event
     * type.
     */
    private static string[] supportedEventNames;
    public static string[] SupportedEventNames {
      get {
        if(null != supportedEventNames)
          return supportedEventNames;
        supportedEventNames =
          SupportedEvents.Select(t => t.ToString()).ToArray();
        return supportedEventNames;
      }
    }

    public static void EnsureEditorsAreReady() {
      if(null != editors)
        return;
      UpdateKnownEditors();
    }

    /**
    * \brief
    * Finds all implementations of IScript with the EventAttribute
    * attribute.
    *
    * This set of types maybe be larger that the set of *editable*
    * event types, since it is possible to define a type decorated
    * with EventAttribute without also defining an editor for it
    * (namely, a subclass of ScriptedEventItemEditor decorated with
    * `ScriptedEventItemEditorFor(target = ...)`).
    */
    private static IEnumerable<Type> GetSupportedEventTypes() =>
      SubtypeReflector
      .GetImplementations<IScript, IScript>()
      .WithAttribute<EventAttribute>();

    public static IEnumerable<Tuple<Type, Type>> GetEditorTypes() {
      var foo = 
        SubtypeReflector
        .GetImplementations<ScriptedEventItemEditor, ScriptedEventItemEditor>();

      Debug.LogFormat("l = {0}", foo.ToArray().Length);

      return
        foo
        .SelectWhere(
          t =>
          t.FindCustomAttribute<ScriptedEventItemEditorFor>()?.FindNamedArgument<Type>("target")
          .Map(
            attr =>
            Tuple.Create(attr, t)));
    }

    /**
     * \brief
     * Finds the editor for each event type, and associates that type
     * with its editor type.
     */
    public static Dictionary<Type, Type> GetEditorDictionary() =>
      // all implementations of ScriptedEventItemEditor with the
      // ScriptedEventItemEditor attribute.
      GetEditorTypes()
      .ToDictionary(
        tup => tup.Item1,
        tup => tup.Item2);

    private static void UpdateKnownEditors() {
      editors = GetEditorDictionary();
      Debug.LogFormat(
        "Editor map updated.\n" +
        "{0}",
        String.Join(
          "\n",
          editors.Select(
            kvp =>
            String.Format("{0} -> {1}", kvp.Key, kvp.Value))));
    }

    /**
     * \brief
     * Creates a new editor for the given type.
     *
     * \returns
     * The editor instance, or null if there is none for the given
     * type.
     */
    public static ScriptedEventItemEditor GetEditor<T>() =>
      GetEditorClass<T>()?.DefaultConstruct<ScriptedEventItemEditor>();

    /**
     * \brief
     * Creates a new editor according to an index into the
     * #SupportedEvents array.
     *
     * \returns
     * The new editor object.
     */
    public static ScriptedEventItemEditor GetEditor(int i) =>
      GetEditor(SupportedEvents[i]);
    /**
     * \brief
     * Creates a new editor for the given type.
     *
     * \returns
     * The editor instance, or null if there is none for the given
     * type.
     */
    public static ScriptedEventItemEditor GetEditor(Type t) =>
      GetEditorClass(t)?.DefaultConstruct<ScriptedEventItemEditor>();

    /**
     * \brief
     * Gets the editor class for the given type.
     *
     * \returns
     * The editor instance, or null if there isn't one.
     */
    public static Type GetEditorClass<T>() =>
      GetEditorClass(typeof(T));

    /**
     * \brief
     * Gets the editor for the given type.
     *
     * It is preferable to call the generic version of this method.
     *
     * \returns
     * The editor instance, or null if there isn't one.
     */
    public static Type GetEditorClass(Type t) {
      EnsureEditorsAreReady();

      Type e;
      return editors.TryGetValue(t, out e) ? e : null;
    }
  }
}
