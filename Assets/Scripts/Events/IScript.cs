namespace SixteenFifty.EventItems {
  using Commands;

  /**
   * \brief
   * Scripted event code.
   *
   * This code is compiled into the Command monad, and ultimately
   * executed as a coroutine.
   */
  public interface IScript {
    Command<object> GetScript(EventRunner runner);

    // T Accept<T>(ScriptVisitor<T> visitor);
  }
}
