using System;
using System.Collections.Generic;

using UnityEngine;

using Commands;

[EventAttribute(friendlyName = "Give Item")]
public class GiveItemScript : EventScript {
  public Item item;
  public int count;

  public override Command<object> GetScript(EventRunner runner) {
    return Command<EventRunner>.Pure(() => runner).ThenAction(Do);
  }

  private void Do(EventRunner runner) {
    Debug.Log("giving item from script");
    var r = runner.Player.inventory.AddItem(item, count);
    if(!r) {
      Debug.Log("Can't add item because no room!");
    }
  }
}
