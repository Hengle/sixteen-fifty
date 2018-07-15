using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Reflection;
  
  /**
   * \brief
   * Maintains state for populating subtype selectors.
   */
  [Serializable]
  public class SubtypeSelectorContext<T> where T : class {
    /**
     * \brief
     * Maps selectable types to their editors.
     */
    Dictionary<Type, Type> editors;

    private static string GetSelectableSubtypeFriendlyName(Type type) =>
      type.FindCustomAttribute<SelectableSubtype>()
      ?.FindNamedArgument<string>("friendlyName")
      ?.Nullify();

    /**
     * \brief
     * A list of all `T` implementations with the SelectableSubtype attribute.
     */
    private Type[] selectableTypes;
    public Type[] SelectableTypes {
      get {
        if(null != selectableTypes)
          return selectableTypes;
        var types = GetSelectableTypes().ToList();
        types.Sort(
          (x, y) =>
          Comparer<string>.Default.Compare(
            GetSelectableSubtypeFriendlyName(x),
            GetSelectableSubtypeFriendlyName(y)));
        return selectableTypes = types.ToArray();
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
    private string[] selectableTypeNames;
    public string[] SelectableTypeNames {
      get {
        if(null != selectableTypeNames)
          return selectableTypeNames;
        selectableTypeNames =
          SelectableTypes.Select(
            t =>
            GetSelectableSubtypeFriendlyName(t))
          .ToArray();
        return selectableTypeNames;
      }
    }

    /**
    * \brief
    * Finds all implementations of `T` with the SelectableSubtype
    * attribute.
    *
    * This set of types maybe be larger that the set of *editable*
    * types, since it is possible to define a type decorated
    * with SelectableSubtype without also defining an editor for it
    * (namely, a subclass of ISubtypeEditor decorated with
    * `SubtypeEditorFor(target = ...)`).
    */
    private static IEnumerable<Type> GetSelectableTypes() =>
      SubtypeReflector
      .GetImplementations<T, T>()
      .WithAttribute<SelectableSubtype>();

    public static IEnumerable<Tuple<Type, Type>> GetEditorTypes() {
      var foo = 
        SubtypeReflector
        .GetImplementations<ISubtypeEditor<T>, ISubtypeEditor<T>>()
        .WithAttribute<SubtypeEditorFor>();

      Debug.LogFormat("l = {0}", foo.ToArray().Length);

      foreach(var t in foo) {
        var target = GetEditorTargetType(t);
        if(null != target)
          yield return Tuple.Create(target, t);
      }
    }

    private static Type GetEditorTargetType(Type editorType) {
      var targets =
        editorType.FindCustomAttribute<SubtypeEditorFor>()
        ?.FindNamedArgument<Type>("target")
        .ToArray();
      if(targets.Length == 0)
        return null;
      var target = targets[0];
      // the target type could be generic, in which case we need to
      // make an instantiation.
      if(!target.IsGenericTypeDefinition)
        // if it isn't a GTD then we can just return it as-is.
        return target;
      // the motivating example:
      // [SubtypeEditorFor(target = Constant<>)]
      // class ConstantEditor<T> : ISubtypeEditor<IExpression<T>>
      // By convention, an editor for a generic type must have a
      // parameter list that matches its target type.
      // If we have a concrete e.g. ConstantEditor<bool> then we
      // instantiate the target Constant<> with the same arguments as
      // its editor type in order to obtain the true target type.
      return target.MakeGenericType(editorType.GetGenericArguments());
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

    public void EnsureEditorsAreReady() {
      if(null != editors)
        return;
      UpdateKnownEditors();
    }

    private void UpdateKnownEditors() {
      editors = GetEditorDictionary();
      Debug.LogFormat(
        "Editor map for {0} updated.\n" +
        "{1}",
        typeof(T),
        String.Join(
          "\n",
          editors.Select(
            kvp =>
            String.Format("{0} -> {1}", kvp.Key, kvp.Value))));
    }

    /**
     * \brief
     * Creates a new editor according to an index into the
     * #SupportedEvents array.
     *
     * \returns
     * The new editor object.
     */
    public ISubtypeEditor<T> GetEditor(int i) =>
      GetEditor(SelectableTypes[i]);
    
    /**
     * \brief
     * Creates a new editor for the given type.
     *
     * \returns
     * The editor instance, or null if there is none for the given
     * type.
     */
    public ISubtypeEditor<S> GetEditor<S>() where S : T {
      var cls = GetEditorClass<S>();
      // tries the single-argument constructor and then the
      // no-argument constructor
      var obj =
        cls?.Construct<SubtypeSelectorContext<T>, ISubtypeEditor<S>>(this)
        ??
        cls?.Construct<ISubtypeEditor<S>>();
      return obj;
    }

    /**
     * \brief
     * Creates a new editor for the given type.
     *
     * \returns
     * The editor instance, or null if there is none for the given
     * type.
     */
    public ISubtypeEditor<T> GetEditor(Type t) {
      var cls = GetEditorClass(t);
      var obj =
        cls?.Construct<SubtypeSelectorContext<T>, ISubtypeEditor<T>>(this)
        ??
        cls?.Construct<ISubtypeEditor<T>>();
      return obj;
    }

    /**
     * \brief
     * Gets the editor class for the given type.
     *
     * \returns
     * The editor instance, or null if there isn't one.
     */
    public Type GetEditorClass<S>() where S : T =>
      GetEditorClass(typeof(S));

    /**
     * \brief
     * Gets the editor for the given type.
     *
     * It is preferable to call the generic version of this method.
     *
     * \returns
     * The editor instance, or null if there isn't one.
     */
    public Type GetEditorClass(Type t) {
      EnsureEditorsAreReady();

      Type e;
      return editors.TryGetValue(t, out e) ? e : null;
    }

  }
}
