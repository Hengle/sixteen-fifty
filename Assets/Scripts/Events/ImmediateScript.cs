using System;

namespace SixteenFifty.EventItems {
  using Commands;
  /**
   * \brief
   * An immediate script just executes a function.
   *
   * Use this for event items that don't take place over time.
   */
  [Serializable]
  public abstract class ImmediateScript : IScript {
    public Command<object> GetScript(EventRunner runner) {
      return
        Command<EventRunner>.Pure(() => runner)
        .ThenAction(Call);
    }

    public abstract void Call(EventRunner runner);

    public abstract bool Equals(IScript that);
  }
}
