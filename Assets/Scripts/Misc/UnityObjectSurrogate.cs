using System;
using System.Runtime.Serialization;

using UnityEngine;

namespace SixteenFifty.Serialization {
  public class UnityObjectSurrogate : ISerializationSurrogate {
    StrObjDict objectFields;

    /**
     * \brief
     * Constructs a UnityObjectSurrogate that stores object references
     * inside the given dictionary.
     */
    public UnityObjectSurrogate(StrObjDict objectFields) {
      this.objectFields = objectFields;
    }

    public void GetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context) {
      string fieldName = context.Context as string;
      // because of the way the surrogate is registered, it is linked
      // to the UnityEngine.Object type:
      // that's the only type `obj` could ever really have.
      objectFields[fieldName] = obj as UnityEngine.Object;
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector) {
      string fieldName = context.Context as string;
      return objectFields[fieldName];
    }
  }
}
