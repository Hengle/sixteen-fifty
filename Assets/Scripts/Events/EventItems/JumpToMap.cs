using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.EventItems {
  using Commands;
  using TileMap;

  [Serializable]
  [SelectableSubtype(friendlyName = "Jump to Map")]
  public class JumpToMap : ImmediateScript, IEquatable<JumpToMap> {
    public BasicMap map;

    public override void Call(EventRunner runner) {
      runner.GridManager.LoadMap(map);
    }

    public bool Equals(JumpToMap that) =>
      map == that.map;

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
