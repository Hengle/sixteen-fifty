using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  [CustomEditor(typeof(ScriptedEvent))]
  public class ScriptedEventEditor : UnityEditor.Editor {
    private static Type[] knownScripts;

    /**
    * \brief
    * Finds all implementations of IScript with the EventAttribute
    * attribute.
    */
    public static IEnumerable<Type> GetSupportedEventTypes() {
      return
        SubtypeReflector
        .GetImplementations<IScript>()
        .Where(
          type =>
          type.CustomAttributes
          .Any(a => a.AttributeType == typeof(EventAttribute)));
    }

    public static void UpdateKnownEventScripts() {
      if(null != knownScripts)
        return;

      knownScripts = GetSupportedEventTypes().ToArray();
    }

    public override void OnInspectorGUI() {
      var target = this.target as ScriptedEvent;
      Debug.Assert(
        null != target,
        "ScriptedEventEditor target is a ScriptedEvent.");


    }
  }
}
