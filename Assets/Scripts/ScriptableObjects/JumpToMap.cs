﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using Commands;
  using TileMap;

  [CreateAssetMenu(menuName = "1650/Events/JumpToMap")]
  public class JumpToMap : EventScript {
    public HexMap map;
    public HexCoordinates coords;

    public override Command<object> GetScript(EventRunner runner) {
      return Command<object>
        .Action(
          () => {
            var hexGridManager = StateManager.Instance.hexGridManager;
            hexGridManager.LoadMap(map);
            var player = hexGridManager.SpawnPlayer();
            player.mapEntity.Warp(coords);
          });
    }
  }
}
