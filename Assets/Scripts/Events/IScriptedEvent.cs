namespace SixteenFifty {
  using EventItems;
  using Serialization;

  /**
   * \brief
   * Scripted events.
   */
  public interface IScriptedEvent {
    IScript Compile();
  }

  /**
   * \brief
   * Unifies SerializableScriptableObject and IScriptedEvent.
   */
  public abstract class BasicScriptedEvent : SerializableScriptableObject, IScriptedEvent {
    public abstract IScript Compile();
  }
}
