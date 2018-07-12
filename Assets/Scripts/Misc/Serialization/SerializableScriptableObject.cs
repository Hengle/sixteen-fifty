using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace SixteenFifty.Serialization {
  /**
  * \brief
  * A subclass of `ScriptableObject` that implements
  * `ISerializationCallbackReceiver` in order to serialize interface fields.
  *
  * `ScriptableObject` subclasses that require serializing fields of
  * interface type should derive from SerializableScriptableObject.
  */
  public abstract class SerializableScriptableObject :
  ScriptableObject, ISerializationCallbackReceiver {
    public void OnBeforeSerialize() {
      HackSerializer.Serialize(this, objectFields, dataFields);
    }
    public void OnAfterDeserialize() {
      HackSerializer.Deserialize(this, objectFields, dataFields);
    }
    
    [SerializeField] [HideInInspector]
    StrObjDict objectFields = new StrObjDict();
    
    [SerializeField] [HideInInspector]
    StrByteDict dataFields = new StrByteDict();
  }
}
