using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1650/Item")]
public class Item : ScriptableObject {
  public Sprite icon;
  new public string name;
  public int stackingSize;
}
