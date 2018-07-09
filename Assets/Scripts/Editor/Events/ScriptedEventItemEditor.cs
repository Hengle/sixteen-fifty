using System;
using System.Runtime;

namespace SixteenFifty.Editor {
  using EventItems;

  public abstract class ScriptedEventItemEditor : ISubtypeEditor<IScript> {
    public abstract void Draw(IScript target);
    public abstract bool CanEdit(Type type);
  }
}
