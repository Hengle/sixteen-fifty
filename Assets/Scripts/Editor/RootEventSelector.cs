using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

public class EventSelectorEditor {
  private static Type[] knownScripts;

  /**
   * \brief
   * Finds all subtypes of BasicEvent with the EventAttribute
   * attribute.
   */
  public static IEnumerable<Type> GetSupportedEventTypes() {
    return
      SubtypeReflector
      .GetSubtypes<EventScript>()
      .Where(
        type =>
        null !=
        type.CustomAttributes
        .Where(a => a.AttributeType == typeof(EventAttribute))
        .FirstOrSentinel(null));
  }

  public static void UpdateKnownEventScripts() {
    if(null != knownScripts)
      return;
    knownScripts = GetSupportedEventTypes().ToArray();
  }

  Numbered<Type> currentChoice;
  public EventScript rootEvent;

  public EventSelectorEditor(EventScript rootEvent = null) {
    UpdateKnownEventScripts();
    this.rootEvent = rootEvent;
  }

  void UpdateCurrentChoice() {
    if(null == rootEvent)
      currentChoice = new Numbered<Type>(0, null);
    else
      currentChoice = knownScripts
        .Numbering(1)
        .Where(s => s.value == rootEvent.GetType())
        .Single();
    // ^ will throw if there is not exactly one element in the
    // sequence
  }

  /**
   * Returns the type selected by the user or null if none is selected
   * yet.
   * Returns `null` if there is nothing selected.
   */
  public Type Show(UnityEngine.Object target) {
    var typeNames = knownScripts.Select(t => t.ToString());
    var choices = (new [] { "Select" }).Concat(typeNames).ToArray();

    UpdateCurrentChoice();

    var i = EditorGUILayout.Popup(
      "Root event",
      currentChoice.number, choices);

    var oldChoice = currentChoice.number;

    var type = i == 0 ? null : knownScripts[i - 1];
    currentChoice =  new Numbered<Type>(i, type);

    // if we just selected the same thing, nothing to do
    if(i == oldChoice)
      goto end;

    // otherwise we changed the type, so we need to destroy the old
    // event (if any) and create the new one.

    bool requireSave = false;

    // if there was an old event, then let's kill it.
    if(rootEvent != null) {
      UnityEngine.Object.DestroyImmediate(rootEvent, true);
      rootEvent = null;
      requireSave = true;
    }

    // if we selected a legitimate type then we need to instantiate it
    // and save it to the asset
    if(type != null) {
      // otherwise, type refers to the type of EventScript to create.
      rootEvent = (EventScript)ScriptableObject.CreateInstance(type);
      AssetDatabase.AddObjectToAsset(rootEvent, target);
      requireSave = true;
    }

    if(requireSave)
      AssetDatabase.SaveAssets();

  end:

    if(rootEvent != null)
      EditorGUILayout.ObjectField("Script", rootEvent, type, false);

    return type;
  }

}
