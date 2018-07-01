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
    private static Dictionary<Type, ScriptedEventItemEditor> editors;

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
    public static IEnumerable<Type> GetSupportedEventTypes() =>
      SubtypeReflector
      .GetImplementations<IScript>()
      .WithAttribute<EventAttribute>();

    /**
     * \brief
     * Finds the editor for each event type, and associates that type
     * with its editor type.
     */
    public static Dictionary<Type, Type> GetEditorTypes() =>
      // all implementations of ScriptedEventItemEditor with the
      // ScriptedEventItemEditor attribute.
      SubtypeReflector
      .GetSubtypes<ScriptedEventItemEditor>()
      .SelectWhere(
        t =>
        t.FindCustomAttribute<ScriptedEventItemEditorFor>()?.FindNamedArgument<Type>("target")
        .Map(
          attr =>
          Tuple.Create(attr, t)))
      .ToDictionary(
        tup => tup.Item1,
        tup => tup.Item2);

    private static void UpdateKnownEditors() {
      // get the editor types and instantiate the editors
      editors =
        GetEditorTypes()
        .Map<Dictionary<Type, ScriptedEventItemEditor>, Type, Type, ScriptedEventItemEditor>(
          t => t.DefaultConstruct<ScriptedEventItemEditor>());
    }

    /**
    * \brief
    * Gets the editor for the given type.
    *
    * \returns
    * The editor instance, or null if there isn't one.
    */
    public static ScriptedEventItemEditor GetEditor<T>() =>
      GetEditor(typeof(T));

    /**
    * \brief
    * Gets the editor for the given type.
    *
    * It is preferable to call the generic version of this method.
    *
    * \returns
    * The editor instance, or null if there isn't one.
    */
    public static ScriptedEventItemEditor GetEditor(Type t) {
      EnsureEditorsAreReady();

      ScriptedEventItemEditor e;
      return editors.TryGetValue(t, out e) ? e : null;
    }
  }

  [CustomEditor(typeof(ScriptedEvent))]
  public class ScriptedEventEditor : UnityEditor.Editor {
    [SerializeField]
    SelectorControl rootSelector;

    Type[] supportedEventTypes;

    new ScriptedEvent target;
    
    void OnEnable() {
      supportedEventTypes =
        ScriptedEventEditorContext
        .GetSupportedEventTypes()
        .ToArray();

      if(rootSelector == null)
        rootSelector = new SelectorControl(
          "Root event",
          supportedEventTypes
          .Select(
            t => t.ToString())
          .ToArray());

      rootSelector.Changed += OnSelectorChanged;
    }

    void OnDisable() {
      rootSelector.Changed -= OnSelectorChanged;
    }

    void OnSelectorChanged(int i) {
      if(i == -1) {
        target.root = null;
        return;
      }

      var t = supportedEventTypes[i];
      target.root = t.DefaultConstruct<IScript>();
    }
    
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      this.target = base.target as ScriptedEvent;
      Debug.Assert(
        null != target,
        "ScriptedEventEditor target is a ScriptedEvent.");

      rootSelector.Draw();

      // if there's no event selected, then there's nothing left to do.
      if(null == target.root)
        return;

      // otherwise, we (try to) get the editor for that event type.
      var type = target.root.GetType();
      var editor = ScriptedEventEditorContext.GetEditor(type);

      // if there isn't an editor, we produce a warning.
      if(editor == null) {
        EditorGUILayout.LabelField(
          String.Format(
            "No editor for type `{0}`.",
            type.ToString()));
        // and gtfo
        return;
      }

      // otherwise, we draw the inspector on the IScript object.
      editor.DrawInspector(target.root);
    }
  }

  [AttributeUsage(AttributeTargets.Class)]
  public class ScriptedEventItemEditorFor : Attribute {
    public Type target;
  }

  public abstract class ScriptedEventItemEditor {
    public abstract void DrawInspector(object target);
  }
}
