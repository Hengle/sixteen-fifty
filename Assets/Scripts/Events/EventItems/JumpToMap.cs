using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;
  using TileMap;

  [Serializable]
  [SelectableSubtype(friendlyName = "Jump to Map")]
  public class JumpToMap : ImmediateScript {
    public BasicMap map;


    public override void Call(EventRunner runner) {
      var hexGridManager = runner.GridManager;
      hexGridManager.LoadMap(map);
    }
  }
}
