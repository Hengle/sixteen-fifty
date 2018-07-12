using System;

using UnityEngine;

namespace SixteenFifty.Serialization {
  public class SerializableBehaviour :
  MonoBehaviour, ISerializationCallbackReceiver {
    [SerializeField] [HideInInspector]
    StrObjDict objectFields = new StrObjDict();
    
    [SerializeField] [HideInInspector]
    StrByteDict dataFields = new StrByteDict();
    
    public void OnBeforeSerialize() {
      HackSerializer.Serialize(this, objectFields, dataFields);
    }
    public void OnAfterDeserialize() {
      HackSerializer.Deserialize(this, objectFields, dataFields);
    }
  }
}
