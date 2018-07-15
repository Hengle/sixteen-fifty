using System;
using System.Runtime;

using UnityEditor;

namespace SixteenFifty.Editor {
  using EventItems;

  public abstract class ScriptedEventItemEditor : ISubtypeEditor<IScript> {
    public abstract void Draw(IScript target);
    public abstract bool CanEdit(Type type);

    /**
     * \brief
     * Records an undo entry for the change that's about to happen to
     * the ScriptedEvent that's being edited.
     */
    protected static void RecordChange(string name) {
      Undo.RecordObject(ScriptedEventEditor.target, name);
    }

    protected static void ChangeHappened() {
      EditorUtility.SetDirty(ScriptedEventEditor.target);
    }
  }
}
