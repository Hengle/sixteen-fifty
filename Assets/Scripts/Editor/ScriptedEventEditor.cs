using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {

  [CustomEditor(typeof(ScriptedEvent))]
  public class ScriptedEventEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      var target = this.target as ScriptedEvent;
      Debug.Assert(
        null != target,
        "ScriptedEventEditor target is a ScriptedEvent.");

    }
  }
}
