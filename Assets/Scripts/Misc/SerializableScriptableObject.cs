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
        // This selector will succeed for *any* type that derives from
        // UnityEngine.Object.
        // This includes types from UnityEngine.dll as well as my own
        // assembly.
        // Furthermore, by using IsAssignableFrom internally, it works
        // for generics, e.g.
        // class Variable<T> : ScriptableObject, IVariable<T> { ... }
        // is assignable to UnityEngine.Object (via ScriptableObject).
        serializer.SurrogateSelector =
          new AssignableSurrogateSelector(
            typeof(UnityEngine.Object),
            new UnityObjectSurrogate(objectFields));

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
            dataFields[name] = new ByteArray { value = stream.ToArray() };
          }
        }
      }
      // Debug.LogFormat(
      //   "Data fields: {0}; Object fields: {1}.",
      //   dataFields.Count, objectFields.Count);
    }
    
    void Deserialize() {
      foreach(var field in GetInterfaceFields()) {
        object result = null;
        string name = field.Name;
        
        // for storing what we get out of the dictionaries.
        UnityEngine.Object obj;
        ByteArray data;
        
        // try to retrieve the field value as a unity object.
        // it's crucial that we do this first, because when
        // serializing a nested unity object, (e.g. a unity object
        // field inside a serializable basic object,) an entry will
        // be added to both objectFields and dataFields!
        // But the associated value in dataFields will be garbage.
        if(objectFields.TryGetValue(name, out obj)) {
          result = obj;
          // Debug.LogFormat(
          //   "Deserialized field {0} -> {1} from objectFields",
          //   name, result);
        }
        // try to retrieve the field value as a custom-serialized byte
        // array.
        else if(dataFields.TryGetValue(name, out data)) {
          using(var stream = new MemoryStream(data.value)) {
            result = Serializer.Deserialize(stream);
          }
          // Debug.LogFormat(
          //   "Deserialized field {0} -> {1} from dataFields",
          //   name, result);
        }
        else {
          // Debug.LogFormat(
          //   "Failed to deserialize field {0}. It may have been lost, or null.",
          //   name);
        }
        
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
