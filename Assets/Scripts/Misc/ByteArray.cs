using System;

namespace SixteenFifty {
  /**
   * \brief
   * Wraps a `byte[]` value so that it can be used as a concrete
   * object within lists that Unity will be capable of serializing.
   */
  [Serializable]
  public class ByteArray {
    public byte[] value;
  }
}
