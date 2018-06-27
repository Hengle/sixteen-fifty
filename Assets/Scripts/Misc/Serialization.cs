namespace SixteenFifty.Serialization {
  /**
  * \brief
  * Maps strings to Unity `Object` references.
  * Used when general serialization encounters Unity stuff.
  */
  public class StrObjDict : SerializableDictionary<string, UnityEngine.Object> {
  }
  
  /**
  * \brief
  * Maps strings to byte arrays.
  * Used to store the result of serializing non-Unity classes.
  */
  public class StrByteDict : SerializableDictionary<string, byte[]> {
  }
}
