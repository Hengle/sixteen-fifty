using System;
using System.Runtime.Serialization;

namespace SixteenFifty.Serialization {
  /**
   * \brief
   * A surrogate selector that uses an `IsAssignableFrom` check to
   * decide whether to use its surrogate.
   */
  public class AssignableSurrogateSelector : ISurrogateSelector {
    ISurrogateSelector next = null;
    ISerializationSurrogate surrogate;
    Type type;
    
    /**
     * \brief
     * Constructs the AssignableSurrogateSelector.
     *
     * \param type
     * The type to which assignment is checked.
     *
     * \param surrogate
     * The ISerializationSurrogate to return if the check passes.
     */
    public AssignableSurrogateSelector(Type type, ISerializationSurrogate surrogate) {
      this.type = type;
      this.surrogate = surrogate;
    }
    
    public void ChainSelector(ISurrogateSelector next) {
      this.next = next;
    }

    public ISurrogateSelector GetNextSelector() => next;

    public ISerializationSurrogate GetSurrogate(
      Type type,
      StreamingContext context,
      out ISurrogateSelector selector) {
      if(this.type.IsAssignableFrom(type)) {
        selector = this;
        return surrogate;
      }
      else if(null != next) {
        return next.GetSurrogate(type, context, out selector);
      }
      else {
        selector = null;
        return null;
      }
    }
  }
}
