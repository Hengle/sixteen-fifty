using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;
  using TileMap;

  [Serializable]
  public class JumpToMap : ImmediateScript {
    public BasicMap map;

    public override void Call(EventRunner runner) {
      var hexGridManager = runner.GridManager;
      hexGridManager.LoadMap(map);
      // not clear whether spawn player should be here or whether
      // spawning should be taken care of by the event script that
      // performs the jump
      hexGridManager.SpawnPlayer();
    }
  }
}
