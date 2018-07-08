using System;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  [Serializable]
  [EventAttribute(friendlyName = "Give Item")]
  public class GiveItem : ImmediateScript {
    public Item item;
    public int count;
    public Inventory target;

    public override void Call(EventRunner runner) {
      target.AddItem(item, count);
      Debug.Log("Gave item.");
    }
  }
}
