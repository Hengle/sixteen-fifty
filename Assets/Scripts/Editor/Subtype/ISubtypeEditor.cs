using System;
using System.Runtime;

namespace SixteenFifty.Editor {
  using EventItems;
  
  [AttributeUsage(AttributeTargets.Class)]
  public class SubtypeEditorFor : Attribute {
    public Type target;
  }

  public interface ISubtypeEditor<T> {
    /**
     * \brief
     * Draws the editor for the given target object.
     *
     * Modifies the target in-place or assigns to it if a new target
     * must be constructed.
     *
     * \returns
     * Whether the object was changed, either by assigning a new
     * value, or by modification of subproperties.
     */
    bool Draw(ref T target);

    /**
     * \brief
     * Decides whether this editor edits the given type of object.
     */
    bool CanEdit(Type type);
  }
}
