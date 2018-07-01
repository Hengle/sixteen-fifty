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
      Serialize();
    }
    public void OnAfterDeserialize() {
      Deserialize();
    }
    
    [SerializeField]
    StrObjDict objectFields = new StrObjDict();
    
    [SerializeField]
    StrByteDict dataFields = new StrByteDict();
    
    BinaryFormatter serializer;
    BinaryFormatter Serializer {
      get {
        if(null != serializer)
          return serializer;

        serializer = new BinaryFormatter();
        var selector = new SurrogateSelector();

        Action<Type, ISerializationSurrogate> addSurrogate = (type, surrogate) => {
          selector.AddSurrogate(type, new StreamingContext(StreamingContextStates.All), surrogate);
          Debug.LogFormat(
            "Registered surrogate: {0} -> {1}.",
            type,
            surrogate);
        };

        var unityObject = typeof(UnityEngine.Object);
        var unitySurrogate = new UnityObjectSurrogate(objectFields);

        // add the surrogate for raw UnityEngine.Object
        // addSurrogate(unityObject, unitySurrogate);

        // now to add the surrogate for every subclass of
        // UnityEngine.Object.

        // first, get all the subclasses of UnityEngine.Object in both
        // the Unity assembly and in my own assembly.
        var unityTypes = unityObject.Assembly.GetTypes()
          .Concat(GetType().Assembly.GetTypes())
          .Where(t => unityObject.IsAssignableFrom(t));

        foreach(var type in unityTypes) {
          addSurrogate(type, unitySurrogate);
        }

        serializer.SurrogateSelector = selector;
        return serializer;
      }
    }
    
    void Serialize() {
      foreach(var field in GetInterfaceFields()) {
        string name = field.Name;

        var value = field.GetValue(this);
        
        // don't bother serializing nulls:
        // on deserialization, things we can't find in our dictionaries
        // will just be set to null (/ default).
        if(value == null)
          continue;
        
        var obj = value as UnityEngine.Object;
        
        if(null != obj)
          // ^ if the object is in fact a Unity Object
          // (it's not totally clear to me how this check could *ever*
          // pass; `field` comes from `GetInterfaceFields()` which filters
          // based on `FieldType.IsInterface`, so how could a Unity Object
          // ever pass that check? Oh. Imagine a declaration like:
          // `class Foo : ScriptableObject, IFoo { ... }`
          // and a field definition like:
          // `IFoo foo = new Foo()`
          // (although obviously not quite like that because you can't
          // just construct a ScriptableObject with a constructor.))
          objectFields[name] = obj;
        // ^ we store it in the dictionary designated for that purpose,
        // using the field name as the key, since field names are
        // unique within an object.
        else {
          // otherwise, the field is not a Unity Object, so we need to
          // do all the work ourselves to serialize it.
          using (var stream = new MemoryStream()) {
            Serializer.Serialize(stream, value);
            stream.Flush();
            // remove the object from the objectFields dictionary if
            // it's there. This could happen if the interface
            // implementation is changed so that it no longer extends
            // Unity Object.
            objectFields.Remove(name);
            dataFields[name] = stream.ToArray();
          }
        }
      }
    }
    
    void Deserialize() {
      foreach(var field in GetInterfaceFields()) {
        object result = null;
        string name = field.Name;
        
        // for storing what we get out of the dictionaries.
        UnityEngine.Object obj;
        byte[] data;
        
        // try to retrieve the field value as a unity object.
        if(objectFields.TryGetValue(name, out obj))
          result = obj;
        // try to retrieve the field value as a custom-serialized byte
        // array.
        else if(dataFields.TryGetValue(name, out data))
          using(var stream = new MemoryStream(data))
            result = Serializer.Deserialize(stream);
        
        // now result is either the Unity Object, OR the deserialized
        // object, OR null in case the field name did not appear in
        // either dictionary.
        // We set the field value to the retrieved result.
        field.SetValue(this, result);
        // presumably, SetValue converts `null` into `default(T)` (for
        // the right T) if the field has a value type.
      }
    }
    
    /**
     * \brief
     * Enumerates the fields of interface type in this object.
     */
    IEnumerable<FieldInfo> GetInterfaceFields() {
      return GetType()
        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .Where(f => !f.IsDefined(typeof(HideInInspector)) && (f.IsPublic || f.IsDefined(typeof(SerializeField))))
        .Where(f => f.FieldType.IsInterface);
    }
  }
}
