using System;
using System.Runtime;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [AttributeUsage(AttributeTargets.Class)]
  public class ScriptedEventItemEditorFor : Attribute {
    public Type target;
  }

  public interface ScriptedEventItemEditor {
    /**
     * \brief
     * Draws the editor for the given target object.
     */
    void DrawInspector(IScript target);

    /**
     * \brief
     * Decides whether this editor edits the given type of object.
     */
    bool CanEdit(Type type);
  }
}
