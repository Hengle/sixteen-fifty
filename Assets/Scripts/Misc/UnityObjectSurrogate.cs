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
      var obj = _obj as UnityEngine.Object;
      // Debug.LogFormat("_obj: {0}; isnull = {1}; type = {2}", _obj, _obj == null, _obj?.GetType());
      Debug.Assert(
        // this condition is weird:
        // the original object is allowed to be null, in which case
        // it's natural for the cast to come out null,
        // but if _obj is *not null*, then obj had better be not null
        // too!
        _obj == null || obj != null,
        String.Format(
          "UnityObjectSurrogate's target `{0}` is a Unity Object.",
          _obj));

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
