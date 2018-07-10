namespace SixteenFifty.EventItems.Expressions {
  /**
   * \brief
   * Represents a computation that produces a result of type `T`.
   *
   * This is used by the scripted events system.
   */
  public interface IExpression<T> {
    T Compute(EventRunner runner);
  }
}
