using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace SixteenFifty.Serialization {
  public class Vector2Surrogate : ISerializationSurrogate {
    public void GetObjectData(
      object _obj,
      SerializationInfo info,
      StreamingContext context) {
      Debug.Assert(
        _obj is Vector2,
        String.Format(
          "Vector2Surrogate's target `{0}` is a Vector2.",
          _obj));
      var obj = (Vector2)_obj;

      info.AddValue("x", obj.x);
      info.AddValue("y", obj.y);
    }

    public object SetObjectData(
      object _obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector) {
      var obj = (Vector2)_obj;

      obj.x = info.GetSingle("x");
      obj.y = info.GetSingle("y");

      return obj;
    }
  }
}
