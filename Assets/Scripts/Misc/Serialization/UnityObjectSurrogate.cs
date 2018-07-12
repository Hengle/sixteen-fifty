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
        object _obj,
        SerializationInfo info,
        StreamingContext context) {
      Debug.Assert(
        _obj is UnityEngine.Object,
        String.Format(
          "UnityObjectSurrogate's target `{0} : {1}` is a Unity Object.",
          _obj,
          _obj?.GetType()));
      var obj = _obj as UnityEngine.Object;

      // we use the instance ID of the key, since context.Context is
      // null. Once again, vexe's code is bogus.
      var id = obj.GetInstanceID();
      objectFields[id.ToString()] = obj;
      info.AddValue("id", id);
    }

    public object SetObjectData(
        object obj,
        SerializationInfo info,
        StreamingContext context,
        ISurrogateSelector selector) {
      var id = info.GetValue("id", typeof(int));
      return objectFields[id.ToString()];

    }
  }
}
