using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  public static class EditorGUILayoutX {
    /**
    * \brief
    * Shows a control to modify a StatValue in-place.
    */
    public static void StatValueField(LineRectManager lmgr, StatValue value) {
      value.value = EditorGUI.FloatField(
        lmgr.NextLine().WithHeight(lmgr.LineHeight),
        "stat value",
        value.value);

      value.statFormatter =
        EditorGUI.ObjectField(
          lmgr.NextLine().WithHeight(lmgr.LineHeight),
          "stat formatter",
          value.statFormatter,
          typeof(StatValueFormatter),
          false)
        as StatValueFormatter;
    }
  }
}
