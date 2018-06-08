using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;

[Serializable]
public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver {
  [SerializeField]
  private List<K> keys = new List<K>();

  [SerializeField]
  private List<V> values = new List<V>();

  [SerializeField]
  private int count = 0;

  public SerializableDictionary() : base() {
  }

  public SerializableDictionary(IDictionary<K, V> dict) : base(dict) {
  }

  [Obsolete("Manual call not supported.", true)]
  public void OnBeforeSerialize() {
    keys.Clear();
    keys.Capacity = Count;
    values.Clear();
    values.Capacity = Count;
    foreach(var p in this) {
      keys.Add(p.Key);
      values.Add(p.Value);
    }
    count = Count;
  }

  public void OnAfterDeserialize() {
    Clear();

    Action<string> die = typeName => {
      throw new SerializationException(
        String.Format("{0} failed to (de)serialize.", typeName));
    };

    if(count != keys.Count)
      die(typeof(K).ToString());
    if(count != values.Count)
      die(typeof(V).ToString());

    for(var i = 0; i < count; i++) {
      Add(keys[i], values[i]);
    }

    keys.Clear();
    values.Clear();
  }
}
