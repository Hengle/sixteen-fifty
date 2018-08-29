using System;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  [Serializable]
  [SelectableSubtype(friendlyName = "Give Item")]
  public class GiveItem : ImmediateScript, IEquatable<GiveItem> {
    public Item item;
    public int count;
    public Inventory target;

    public override void Call(EventRunner runner) {
      target.AddItem(item, count);
      Debug.Log("Gave item.");
    }

    public bool Equals(GiveItem that) =>
      item == that.item &&
      count == that.count &&
      target == that.target;


    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
