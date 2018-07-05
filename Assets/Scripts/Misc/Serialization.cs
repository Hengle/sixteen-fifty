using System;

namespace SixteenFifty.Serialization {

  /**
  * \brief
  * Maps strings to Unity `Object` references.
  * Used when general serialization encounters Unity stuff.
  */
  [Serializable]
  public class StrObjDict : SerializableDictionary<string, UnityEngine.Object> {
  }
  
  /**
  * \brief
  * Maps strings to byte arrays.
  * Used to store the result of serializing non-Unity classes.
  */
  [Serializable]
  public class StrByteDict : SerializableDictionary<string, ByteArray> {
  }
}
