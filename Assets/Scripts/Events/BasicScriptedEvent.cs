namespace SixteenFifty {
  using EventItems;
  using Serialization;

  /**
   * \brief
   * Unifies SerializableScriptableObject and IScriptedEvent.
   */
  public abstract class BasicScriptedEvent : SerializableScriptableObject, IScriptedEvent {
    public abstract IScript Compile();
  }
}
