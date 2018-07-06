using System;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  [Serializable]
  [EventAttribute(friendlyName = "Give Item")]
  public class GiveItem : ImmediateScript {
    public Item item;
    public int count;

    public override void Call(EventRunner runner) {
      runner.GridManager.Player.inventory.AddItem(item, count);
      Debug.Log("Gave item.");
    }
  }
}
