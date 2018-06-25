using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty {
  /**
   * \brief
   * Represents an asset that was dynamically loaded.
   */
  public class AssetInfo<T> where T : ScriptableObject {
    public string path;
    public string guid;
    public T asset;
  }
  
  namespace Editor {
    public static class EditorMisc {
      public static IEnumerable<AssetInfo<T>> GetAllInstances<T>() where T : ScriptableObject {
        var name = typeof(T).Name;
    
        return
          AssetDatabase.FindAssets("t:" + name)
          .Select(
            guid => {
              var path = AssetDatabase.GUIDToAssetPath(guid);
              var asset = AssetDatabase.LoadAssetAtPath<T>(path);
              return new AssetInfo<T> { path = path, asset = asset, guid = guid };
            });
      }
    }
  }
}
