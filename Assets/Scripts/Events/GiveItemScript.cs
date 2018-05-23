using System;
using System.Collections.Generic;

using UnityEngine;

using Commands;

public class GiveItemScript : BasicScript {
  public Item item;
  public int count;

  public override Command<object> GetScript(EventRunner runner) {
    return Command<EventRunner>.Pure(() => runner).ThenAction(Do);
  }

  private void Do(EventRunner runner) {
    var r = runner.Inventory.AddItem(item, count);
    if(!r) {
      Debug.Log("Can't add item because no room!");
    }
  }
}
