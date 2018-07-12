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
}
