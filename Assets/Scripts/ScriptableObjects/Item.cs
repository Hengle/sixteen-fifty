using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SixteenFifty {
  using Serialization;
  
  [CreateAssetMenu(menuName = "1650/Item")]
  public class Item : SerializableScriptableObject {
    public Sprite icon;
    new public string name;
    public int stackingSize;
  }
}
