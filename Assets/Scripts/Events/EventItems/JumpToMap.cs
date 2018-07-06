using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;
  using TileMap;

  [Serializable]
  public class JumpToMap : ImmediateScript {
    public HexMap map;
    public HexCoordinates coords;

    public override void Call(EventRunner runner) {
      var hexGridManager = runner.GridManager;
      hexGridManager.LoadMap(map);
      var player = hexGridManager.SpawnPlayer();
      player.mapEntity.Warp(coords);
    }
  }
}
